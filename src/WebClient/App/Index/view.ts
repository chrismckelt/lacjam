/// <reference path="../_references.ts" />
module app.controllers {

    import IndexModel = app.controllers.ModelBase
    export class IndexModel implements app.controllers.ModelBase {

    }

    export interface IIndexScope extends IScopeBase{
        vm: IndexModel;
    }

    export class Index extends ControllerBase<IndexModel> {
        scope: IIndexScope;
        constructor( $injector : ng.auto.IInjectorService,$scope: IIndexScope) {
            super("Index", $scope);
            this.scope = $scope;
            this.scope.vm = new IndexModel();
 
        }
    }
}

app.registerController("app.controllers.Index", []);