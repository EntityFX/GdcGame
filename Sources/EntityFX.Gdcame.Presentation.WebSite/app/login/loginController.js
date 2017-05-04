/// <reference path="../../Scripts/typings/angularjs/angular.d.ts" />
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var Gdcame;
(function (Gdcame) {
    var Controllers;
    (function (Controllers) {
        var LoginController = (function (_super) {
            __extends(LoginController, _super);
            function LoginController($location, authenticationService) {
                var _this = _super.call(this) || this;
                _this.user = { login: 'admin', password: "P@ssw0rd" };
                _this.location = $location;
                _this.authenticationService = authenticationService;
                return _this;
            }
            LoginController.prototype.login = function () {
                var _this = this;
                this.authenticationService.login(this.user)
                    .then(function (value) {
                    _this.location.path('/');
                }, function (error) {
                    _this.showMessage("error", error.data);
                });
            };
            return LoginController;
        }(Controllers.ControllerBase));
        angular.module("gdCameApp").controller("LoginController", LoginController);
        LoginController.$inject = [
            "$location", "AuthDataService"
        ];
    })(Controllers = Gdcame.Controllers || (Gdcame.Controllers = {}));
})(Gdcame || (Gdcame = {}));
//# sourceMappingURL=LoginController.js.map