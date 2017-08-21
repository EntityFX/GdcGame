namespace Gdcame.Controllers {
    import Controller = angular.IController;

    export abstract class ControllerBase implements Controller {
        public $onInit(): void { }

        public showMessage(type: any, message: String): void {
            alert(message);
        }

        public hideMessage(): void {

        }

        protected validateForm(form) {
            if (!form.$valid) {
                angular.forEach(form,
                    (value, key) => {
                        if (typeof value === 'object' && value.hasOwnProperty('$modelValue') && !value.$valid)
                            value.$setDirty();
                    });
                return false;
            }
            return true;
        };

        protected setPristineFormsState(scope, formNamesList) {
            if (!formNamesList)
                return;
            formNamesList.forEach(formName => {
                var parts = formName.split('.');
                var formElem = scope;
                parts.forEach(part => {
                    formElem = formElem[part];
                });
                if (formElem && formElem.$valid) {
                    formElem.$setPristine();
                }
            });
        };
    }
}