


Demo video: docs/demo.mp4

## Frontend
### Environment
npm v8.5.5 

NodeJs v16.15.0

ng/cli v16.2.2

### Running
1. Make sure you have installed the npm packages and angular cli.

2. Then run this command to start the angular
```
ng serve
```
3. Access this url in browser
```
http://localhost:4200/
```

### Test
```
ng test
```
Test result example: docs/Karma.png

### Configuration


host port can be changed in \Frontend\src\environment.ts
```
export const environment = {
	production: false,
	apiUrl: 'http://localhost:5214/api',
};
```

## Backend

### Environment
.NET 8

Visual Studio 2022 Latest Version

### Running
1. 
```
dotnet run
```
2. The saved data can be found in Backend\myapi\myapi\Data\UserInfo.json


### Test
Run test in the Test Explorer in Visual Studio.

Test result example: docs/xunit-Test-result.png

postman test: docs/CreateNewUserApiCollection.postman_collection.json

## Security Concern
Currently, the Https transmission protocol and authentication and authorization mechanisms are not enabled, which may lead to data leakage risks. Please use with caution.