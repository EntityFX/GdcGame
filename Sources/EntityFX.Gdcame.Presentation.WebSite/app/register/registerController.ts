/// <reference path="../../Scripts/typings/angularjs/angular.d.ts" />

namespace Gdcame.Controllers {
    import LocationService = angular.ILocationService;
    import RegisterDataService = Services.IRegisterDataService;
    import RegisterData = Gdcame.Services.RegisterData;

    class RegisterController extends Controllers.ControllerBase {
        public registerData: RegisterData = {} as RegisterData;
        private location: LocationService;
        private registerDataService: RegisterDataService;

        constructor($location: LocationService, registerDataService: RegisterDataService) {
            super();
            this.location = $location;
            this.registerDataService = registerDataService;
        }

        public register() {
            this.registerDataService.register(this.registerData)
                .then((response) => {
                        this.location.path('/login');
                    },
                    response => {
                        alert(JSON.stringify(response.message));
                    });
        };

    }

    angular.module("gdCameApp").controller("RegisterController", RegisterController);

    RegisterController.$inject = [
        "$location", "RegisterDataService"
    ];
}