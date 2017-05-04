module Gdcame.Services {
    import HttpService = angular.IHttpService;
    export class GameApiService extends ApiServiceBase implements Services.IGameApiService {
        private apiServiceUri: Services.IUriService;

        constructor($http: HttpService, apiServiceUri: Services.IUriService) {
            super($http);
            this.apiServiceUri = apiServiceUri;
        }

        getGameData(): angular.IHttpPromise<Gdcame.Services.GameData> {
            return this.http.get(`${this.apiServiceUri.getSesionApiAddress()}/api/game/game-data/`);
        }

        getCounters(): angular.IHttpPromise<Gdcame.Services.Cash> {
            return this.http.get(`${this.apiServiceUri.getSesionApiAddress()}/api/game/counters/`);
        }

        performManualStep(verificationNumber: number): angular.IHttpPromise<Gdcame.Services.ManualStepResult> {
            return this.http.post(`${this.apiServiceUri.getSesionApiAddress()}/api/game/perform-step/`, verificationNumber);
        }

        buyFundDriver(fundDriverId: number): angular.IHttpPromise<Gdcame.Services.BuyItem> {
            return this.http.post(`${this.apiServiceUri.getSesionApiAddress()}/api/game/buy-item/`, fundDriverId);
        }

        fightAgainstInflation(): angular.IHttpPromise<Gdcame.Services.Cash> {
            return this.http.post(`${this.apiServiceUri.getSesionApiAddress()}/api/game/fight-inflation/`, {});
        }

        activateDelayedCounter(counterId: number): angular.IHttpPromise<Gdcame.Services.Cash> {
            return this.http.post(`${this.apiServiceUri.getSesionApiAddress()}/api/game/ActivateDelayedCounter/`, counterId);
        }
    }

    angular.module("gdCameApp").service("GameApiService", GameApiService);

    GameApiService.$inject = [
        "$http", "UriService"
    ];
}