/// <reference path="../types.d.ts" />
/// <reference path="../../Scripts/typings/angularjs/angular.d.ts" />

namespace Gdcame.Controllers {
    import SessionService = Gdcame.Services.ISessionService;

    class MenuController extends Controllers.ControllerBase
    {
        public isAuthenticated: boolean;
        private scope: angular.IScope;
        private sessionService: SessionService;

        constructor($scope: angular.IScope, sessionService: SessionService) {
            super();
            this.scope = $scope;
            this.sessionService = sessionService;
            this.scope.$on("onLoginSuccess", (event) => this.onLoginSuccessHandler(event));
        }

        private onLoginSuccessHandler(event: angular.IAngularEvent, ...args: any[]): any {
            this.isAuthenticated = this.sessionService.getLogin() != null;
        }
    }

    angular.module("gdCameApp").controller('MenuController', MenuController);

    MenuController.$inject = ["$scope", "AuthDataService"];

}