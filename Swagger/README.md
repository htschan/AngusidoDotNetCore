# Swagger

`npm install -g swagger`

Put the Swagger specification `swagger.yaml` into this folder: `./api/swagger`

To edit the specification:

`swagger project edit`

# Create gulp command to convert yaml to json

The swagger.json file will be used by the Nswag tool below to generate code.

`npm install --save-dev js-yaml`

```javascript
gulp.task('swagger', () => {
  var doc = yaml.safeLoad(fs.readFileSync(path.join(__dirname, "./api/swagger/swagger.yaml")));
  fs.mkdir("./dist");
  fs.writeFileSync(path.join(__dirname, "./dist/swagger.json"), JSON.stringify(doc, null, " "));
});
```


## Nswag commandline

`npm install --save-dev nswag`


## Install Nswag Studio 

To prepare an *.nswag file for automated generation of client and server code.

https://github.com/NSwag/NSwag/wiki/NSwagStudio


## Generate code

`nswag run`

# Generating code for APS.NET Core

## GUI / Settings Parameter

This option turns on the ASP.NET Core generation

x Generate ASP.NET Core

When this option is set, the behavior of the code generator is as follows:

swagger.yaml

### summary Operation Attribute extension
  AuthenticationAttibute: [AllowAnonymous]
    Specify anonymous access for operation
  AuthorizeAttribute:  [Authorize(Policy = "RequireApiAdminRole")]
    Specify required authorization policy for method where "[Authorize(Policy" is the keyword

### Generate model binding attribute
  Decorate method parameters with ModelBinding Attributes, e.g, [FromBody], [FromRoute] etc.

### File Upload

#### Single file upload
      parameters:
        - name: document
          in: formData
          description: The content of the file
          required: true
          type: file

#### Multiple file upload
      parameters:
        - name: document
          in: formData
          description: The content of the file
          required: true
          type: file
          collectionFormat: multi



