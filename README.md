# FAExportLib

This is a Visual Basic (.NET Framework) library that interfaces with FAExport at https://faexport.boothale.net to allow access to FurAffiniy submissions.

There is currently no way to log in; all users and submissions will be accessible, as if you were a logged in user with no content filter settings.

Supported endpoints:

* /user/{name}
* /user/{name}/{folder}
* /submission/{id}

Dependencies:
* Newtonsoft.Json
