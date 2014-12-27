/// <reference path="../_References.ts" />
// files loaded last

app.registerService(app.services.Common);

app.log.info("Registering services");

app.registerService(app.services.Common);
//app.registerService(app.services.Dialog);
app.registerService(app.services.MetadataDefinitionGroupService);
app.registerService(app.services.MetadataDefinitionService);


app.log.info("Registering controllers");
app.registerController("FooterController", app.controllers.FooterController);
app.registerController("IndexController", app.controllers.IndexController); 
app.registerController("DashboardController", app.controllers.DashboardController); 
app.registerController("MetadataDefinitionGroupController", app.controllers.MetadataDefinitionGroupController); // register 

