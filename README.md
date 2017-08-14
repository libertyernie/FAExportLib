# FAExportLib

This is a Visual Basic (.NET Framework) library that interfaces with FAExport at https://faexport.boothale.net to allow access to FurAffiniy submissions.

If you don't specify "a" and "b" cookie values when creating an FAClient object, all users and submissions will be accessible, as if you were a logged in user with no content filter settings; however, you will not be able to view or modify account-specific data.

Supported endpoints:

* /user/{name}
* /user/{name}/{folder}
* /submission/{id}

Dependencies:
* Newtonsoft.Json
