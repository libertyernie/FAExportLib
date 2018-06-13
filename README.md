# FAExportLib

This is a C# (.NET Framework) library that interfaces with FAExport at https://faexport.boothale.net to allow access to FurAffiniy submissions.

If you want to act as a certain user (which lets you post journals and allows limiting what mature/adult submissions you see), create a FAUserClient object with the "a" and "b" cookies from FurAffinity. You can also just create a FAClient object, where all users and submissions will be accessible.

Supported endpoints:

* GET /user/{name}
* GET /user/{name}/{folder}
* GET /user/{name}/journals
* GET /search
* GET /submission/{id}
* POST /journal

Dependencies:
* Newtonsoft.Json
