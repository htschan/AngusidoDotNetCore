"use strict";

const gulp = require("gulp"),
  del = require("del"),
  fs = require("fs"),
  path = require("path"),
  yaml = require("js-yaml"),
  runSequence = require('run-sequence'),
  runExe = require('child_process').exec;

gulp.task('watch', ['transpile'], () => {
  gulp.watch('./api/swagger/swagger.yaml', ['swagger']);
});

gulp.task('default', ['watch']);

gulp.task('copyToAnMaterial', (cb) => {
	gulp
	.src('../Ionic/src/services/api.g.ts')
	.pipe(gulp.dest('../AnMaterial/src/app/services', {overwrite: true}))
});

gulp.task('generate', (cb) => {
	runExe('..\\..\\NSwag\\src\\NSwag.Console\\bin\\Debug\\net461\\NSwag.exe run swagger.nswag', (err, stdout, stderr) =>{
		console.log(stdout);
		console.log(stderr);
		cb(err);
	});
});

gulp.task('swagger', () => {
  var doc = yaml.safeLoad(fs.readFileSync(path.join(__dirname, "./api/swagger/swagger.yaml")));
  fs.mkdir("./dist");
  fs.writeFileSync(path.join(__dirname, "./dist/swagger.json"), JSON.stringify(doc, null, " "));
});

gulp.task('clean', (cb) => {
  return del(["dist"], cb);
});

gulp.task("build", function (callback) {
  runSequence('clean', 'swagger', 'generate', 'copyToAnMaterial', callback);
});
