angular
    .module("gdCameApp")
    .component("login", {
    templateUrl: function (viewTheme) {
        return "/app/views/" + viewTheme + "/loginView.html";
    },
    controller: 'LoginController',
    controllerAs: "controller"
});
//# sourceMappingURL=loginComponent.js.map