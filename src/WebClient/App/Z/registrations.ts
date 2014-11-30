/// <reference path="../_References.ts" />

// file loaded last

app.registerService(app.services.Common);

app.log.info("Registering services");

app.registerService(app.services.Common);
//app.registerService(app.services.Dialog);
app.registerService(app.services.MetadataDefinitionGroupService);
app.registerService(app.services.MetadataDefinitionService);



app.log.info("Registering controllers");
app.registerController("IndexController", app.controllers.IndexController); // register 
app.registerController("MetadataDefinitionGroupController", app.controllers.MetadataDefinitionGroupController); // register 

