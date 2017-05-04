/// <reference path="../../Scripts/typings/angularjs/angular.d.ts" />

namespace Gdcame.Controllers {
    import AuthData = Services.AuthData;
    import LocationService = angular.ILocationService;
    import AuthenticateService = Services.IAuthenticateService;
    import AuthToken = Services.AuthToken;

    class LoginController extends Controllers.ControllerBase {
        private user: AuthData = { login: 'admin', password: "P@ssw0rd" };
        private location: LocationService;
        private authenticationService: AuthenticateService;

        constructor($location: LocationService, authenticationService: AuthenticateService) {
            super();
            this.location = $location;
            this.authenticationService = authenticationService;
        }

        public login(): void {
            this.authenticationService.login(this.user)
                .then((value : AuthToken) : void => {
                    this.location.path('/');
                }, (error: any) : void => {
                    this.showMessage("error", JSON.stringify(error));
                });
        }
    }

    angular.module("gdCameApp").controller("LoginController", LoginController);

    LoginController.$inject = [
        "$location", "AuthDataService"
    ];
}