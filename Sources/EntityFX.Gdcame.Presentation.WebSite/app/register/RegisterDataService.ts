﻿namespace Gdcame.Services {
    class RegisterDataService implements IRegisterDataService {
        private q: angular.IQService;
        private userService: Services.IUserApiService;

        public constructor(
            $q: angular.IQService,
            userService: Services.IUserApiService) {
            this.userService = userService;
            this.q = $q;
        }

        register(user: Services.RegisterData): angular.IHttpPromise<any> {
            var defer = this.q.defer<Services.AuthToken>();
            this.userService.register(user)
                .then(result => {
                    defer.resolve(result.data);
                }, error => {
                    defer.reject(error.data);
                });
            return defer.promise;
        }
    }


    angular.module("gdCameApp").service("RegisterDataService", RegisterDataService);

    RegisterDataService.$inject = [
        "$q", "UserApiService"
    ];
}