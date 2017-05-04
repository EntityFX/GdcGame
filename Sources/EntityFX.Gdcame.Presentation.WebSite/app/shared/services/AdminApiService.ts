module Gdcame.Services {
    import HttpService = angular.IHttpService;

    export class AdminApiService extends ApiServiceBase implements Services.IAdminApiService {
        constructor($http: HttpService) {
            super($http);
        }

        getStatistics(serverAddress: string): angular.IHttpPromise<Services.ServerStatisticsInfo> {
            return this.http.get(`${serverAddress}/api/admin/statistics/`);
        }

        getActiveSessions(serverAddress: string): angular.IHttpPromise<Services.UserSessions[]> {
            return this.http.get(`${serverAddress}/api/admin/sessions/`);
        }
    }

    angular.module("gdCameApp").service("AdminApiService", AdminApiService);

    AdminApiService.$inject = [
        "$http"
    ];
}