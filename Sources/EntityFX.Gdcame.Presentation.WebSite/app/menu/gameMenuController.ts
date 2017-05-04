/// <reference path="../types.d.ts" />
/// <reference path="../../Scripts/typings/angularjs/angular.d.ts" />
/// <reference path="../shared/ControllerBase.ts" />
namespace Gdcame.Controllers {
    import SessionService = Gdcame.Services.ISessionService;

    class GameMenuController extends Controllers.ControllerBase
    {
        public isAuthenticated: boolean;
        private scope: angular.IScope;
        private sessionService: SessionService;

        constructor($scope: angular.IScope, sessionService: SessionService) {
            super();
            this.scope = $scope;
            this.sessionService = sessionService;
            this.scope.$on("onLoginSuccess", this.onLoginSuccessHandler);
        }

        private onLoginSuccessHandler(event: angular.IAngularEvent, ...args: any[]): any {
            this.isAuthenticated = this.sessionService.getLogin() != null;
        }
    }

    angular.module("gdCameApp").controller('GameMenuController', GameMenuController);

    GameMenuController.$inject = ["$scope", "AuthDataService"];

}