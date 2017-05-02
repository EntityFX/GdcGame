/// <reference path="../../Scripts/typings/angularjs/angular.d.ts" />

namespace Gdcame.Services {
    import HttpService = angular.IHttpService;

    export abstract class ApiServiceBase {
        protected http: HttpService;

        public constructor($http: HttpService) {
            this.http = $http;
        }
    }
}