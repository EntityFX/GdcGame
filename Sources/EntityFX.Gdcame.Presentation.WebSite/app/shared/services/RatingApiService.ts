module Gdcame.Services {
    import HttpService = angular.IHttpService;

    export class RatingApiService extends ApiServiceBase implements Services.IRatingApiService {
        constructor($http: HttpService) {
            super($http);
        }

        getRaiting(serverAddress: string, top: number): angular.IHttpPromise<TopRatingStatisticsModel> {
            return this.http.get(`${serverAddress}/api/rating`, { params: { top: top } });
        }
    }

    angular.module("gdCameApp").service("RatingApiService", RatingApiService);

    RatingApiService.$inject = [
        "$http"
    ];
}