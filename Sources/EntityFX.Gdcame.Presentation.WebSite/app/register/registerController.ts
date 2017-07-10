/// <reference path="../../Scripts/typings/angularjs/angular.d.ts" />

namespace Gdcame.Controllers {
    import LocationService = angular.ILocationService;
    import RegisterDataService = Services.IRegisterDataService;
    import RegisterData = Gdcame.Services.RegisterData;

    class RegisterController extends Controllers.ControllerBase {
        public registerData: RegisterData = {} as RegisterData;
        public error: string;
        public registerInProgress: boolean = false;

        private location: LocationService;
        private registerDataService: RegisterDataService;

        constructor($location: LocationService, registerDataService: RegisterDataService) {
            super();
            this.location = $location;
            this.registerDataService = registerDataService;
        }

        public register() {
            this.error = null;
            this.registerInProgress = true;
            this.registerDataService.register(this.registerData)
                .then((response) => {
                    this.location.path('/login');
                },
                response => {
                    this.error = JSON.stringify(response.message);
                }).finally(() => {
                    this.registerInProgress = false;
                });;
        };

    }

    angular.module("gdCameApp").controller("RegisterController", RegisterController);

    RegisterController.$inject = [
        "$location", "RegisterDataService"
    ];
}