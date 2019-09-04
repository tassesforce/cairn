<template>
    <div>
        <div id="updateAuth" v-if="readOnly === false">
            <div class="equalInlineField">
                <input type="text" name="login" v-model.trim="baseModel.login" placeholder="Login" v-on:blur="validateLogin()" />
                <p v-if="errorLabels.loginError.length > 0" class="error">{{errorLabels.loginError}}</p><br v-else/>
            </div>
            <div class="equalInlineField">
                <input type="password" name="password" v-model.trim="baseModel.password" placeholder="Password" v-on:blur="validatePassword()" />
                <p v-if="errorLabels.passwordError.length > 0" class="error">{{errorLabels.passwordError}}</p><br v-else/>
            </div>
            <!-- <input type="password" name="verifPassword" v-model.trim="verifPassword" v-on:blur="validateVerifPassword()" />
            <p v-if="errorLabels.verifPasswordError.length > 0" class="error">{{errorLabels.verifPasswordError}}</p><br v-else/> -->
        </div>
        <div id="readAuth" v-if="readOnly === true">
            <p>{{baseModel.login}}</p>
        </div>
    </div>
</template>

<script>
export default {
    name: 'Auth',
    data() {
        return {
            passwordMinLength: 8,
            loginMinLength: 4,
            verifPassword: "",
            errorLabels: {
                loginError: "",
                passwordError: "",
                verifPasswordError: ""
            },
            errorMessages: {
                login: {
                    empty: "Merci de saisir un identifiant. ",
                    size: "L'identifiant doit être au minimum de 4 caractères"
                },
                password: {
                    empty: "Merci de saisir un mot de passe. ",
                    minLength: "Le mot de passe doit faire au moins 8 caractères",
                    format: "Le mot de passe doit contenir au moins une lettre majuscule et minuscule, un chiffre et un caractère spécial"
                },
                verifPassword: {
                    different: "Le mot de passe saisi est différent."
                }
            }
        };
    },
    methods: {
        fetchDatas() {
            return this.baseModel;
        },
        validateForm() {
            var valid = true;
            if (!this.validateLogin()) {
                valid = false;
            }
            if (!this.validatePassword()) {
                valid = false;
            }
            // if (!this.validateVerifPassword()) {
            //     valid = false;
            // }
            return valid;
        },
        validateLogin() {
            if (typeof this.baseModel.login == "undefined" || this.baseModel.login.length == 0) {
                this.errorLabels.loginError = this.errorMessages.login.empty;
                return false;
            } else if (this.baseModel.login.length < this.loginMinLength) {
                this.errorLabels.loginError = this.errorMessages.login.size;
                return false;
            } else {
                this.errorLabels.loginError = "";
            }
            return true;
        },
        validatePassword() {
            // 8 characters with at least : 1 letter, 1 number and 1 special character 
            var regExp = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/;
            if (typeof this.baseModel.password == "undefined" || this.baseModel.password.length == 0) {
                this.errorLabels.passwordError = this.errorMessages.password.empty;
                return false;
            } else if (this.baseModel.password.length < this.passwordMinLength) {
                this.errorLabels.passwordError = this.errorMessages.password.minLength;
                return false;
            } else if (!regExp.test(this.baseModel.password)) {
                this.errorLabels.passwordError = this.errorMessages.password.format;
                return false;
            } else if (this.verifPassword != "" && this.baseModel.password != this.verifPassword) {
                this.errorLabels.passwordError = this.errorMessages.verifPassword.different;
                return false;
            } else {
                this.errorLabels.passwordError = "";
            }
            return true;
        },
        validateVerifPassword() {
            if (this.baseModel.password.length != 0 && this.verifPassword != this.baseModel.password) {
                this.errorLabels.verifPasswordError = this.errorMessages.verifPassword.different;
                return false;
            } else {
                this.errorLabels.verifPasswordError = "";
            }
            return true;
        }
    },
    props: ['baseModel', 'readOnly']
}
</script>

<style>

</style>
