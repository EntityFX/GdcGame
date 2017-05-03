module Gdcame.Services {
    export class GameApiService extends ApiServiceBase implements  Services.IGameApiService {
        getGameData(): angular.IHttpPromise<Gdcame.Services.GameData> { throw new Error("Not implemented"); }

        getCounters(): angular.IHttpPromise<Gdcame.Services.Cash> { throw new Error("Not implemented"); }

        performManualStep(verificationNumber: number): angular.IHttpPromise<Gdcame.Services.ManualStepResult> { throw new Error("Not implemented"); }

        buyFundDriver(fundDriverId: number): angular.IHttpPromise<Gdcame.Services.BuyItem> { throw new Error("Not implemented"); }

        fightAgainstInflation(): angular.IHttpPromise<Gdcame.Services.Cash> { throw new Error("Not implemented"); }

        activateDelayedCounter(counterId: number): angular.IHttpPromise<Gdcame.Services.Cash> { throw new Error("Not implemented"); }
    }
}