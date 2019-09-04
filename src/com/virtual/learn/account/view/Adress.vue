<template>
    <div>
        <div id="updateAdress" v-if="readOnly === false">
            <div>
                <div class="smallInlineField">
                <input type="text" name="number" v-model.number="baseModel.number" placeholder="number" v-on:blur="validateNumber()" />
                <p v-if="errorLabels.numberError.length > 0" class="error">{{errorLabels.numberError}}</p><br v-else/>
                </div>
                <div class="bigInlineField">
                <input type="text" name="label" v-model="baseModel.label" placeholder="label"  v-on:blur="validateLabel()"/>
                <p v-if="errorLabels.labelError.length > 0" class="error">{{errorLabels.labelError}}</p><br v-else/>
                </div>
            </div>
            <input type="text" name="complement1" v-model="baseModel.complement1" placeholder="complement1" />
            <input type="text" name="complement2" v-model="baseModel.complement2" placeholder="complement2" />
            <div class="smallInlineField">
                <input type="text" name="postalCode" v-model="baseModel.postalCode" placeholder="postalCode" v-on:blur="validatePostalCode()" />
                <p v-if="errorLabels.postalCodeError.length > 0" class="error">{{errorLabels.postalCodeError}}</p><br v-else/>
            </div>
            <div class="bigInlineField">
                <input type="text" name="town" v-model="baseModel.town" placeholder="town" v-on:blur="validateTown()" />
                <p v-if="errorLabels.townError.length > 0" class="error">{{errorLabels.townError}}</p><br v-else/>
            </div>
        </div>

        <div id="readAdress" v-if="readOnly === true">
            <span>{{baseModel.number}} {{baseModel.label}}, </span>
            <span v-if="typeof baseModel.complement1 != 'undefined' && baseModel.complement1.length > 0">{{baseModel.complement1}} </span>
            <span v-if="typeof baseModel.complement2 != 'undefined' && baseModel.complement2.length > 0">{{baseModel.complement2}}</span>
            <span v-if="(typeof baseModel.complement1 != 'undefined' && baseModel.complement1.length > 0) || (typeof baseModel.complement2 != 'undefined' && baseModel.complement2.length > 0)">, </span>
            <span>{{baseModel.postalCode}} {{baseModel.town}} </span>
            <span v-if="typeof baseModel.country != 'undefined' && baseModel.country.length > 0">({{baseModel.country}})</span>
        </div>
    </div>
</template>

<script>

var regExpNumber = /^[0-9]+$/;
var regExpCountry = /^[A-Za-z\u00C0-\u017F-' ]+$/;
var regExpLabel = /^[A-Za-z0-9\u00C0-\u017F-' ]+$/;

export default {
    name: 'Adress',
    data() {
        return {
            errorLabels: {
                numberError: "",
                labelError: "",
                postalCodeError: "",
                townError: "",
                countryError: ""
            },
            errorMessages: {
                number: {
                    empty: "Le numéro de rue est obligatoire.",
                    zero: "Le numéro de rue ne peut être 0.",
                    negative: "Le numéro de rue ne peut être négatif.",
                    format: "Le numéro de rue doit être un nombre entier."
                },
                label: {
                    empty: "Le libellé de la voie est obligatoire.",
                    format: "Les caractères spéciaux sont interdits dans ce champ."
                },
                postalCode: {
                    empty: "Le code postal est obligatoire.",
                    size: "Le code postal doit faire 5 caractères.",
                    format: "Le code postal ne peut être composé que de chiffres."
                },
                town: {
                    empty: "Le nom de ville est obligatoire.",
                    format: "Le nom de ville ne semble pas correspondre au format attendu (Exemple : Lyon)."
                }
            }
        };
    },
    methods: {
        validateForm() {
            var valid = true;
            if (!this.validateNumber()) {
                valid = false;
            }
            if (!this.validateLabel()) {
                valid = false;
            }
            if (!this.validatePostalCode()) {
                valid = false;
            }
            if (!this.validateTown()) {
                valid = false;
            }
            return valid;
        },
        validateNumber() {
            if (typeof this.baseModel.number == "undefined" || this.baseModel.number == 0) {
                this.errorLabels.numberError = this.errorMessages.number.empty;
                return false;
            } else if (this.baseModel.number == 0) {
                this.errorLabels.numberError = this.errorMessages.number.zero;
                return false;
            } else if (this.baseModel.number < 0) {
                this.errorLabels.numberError = this.errorMessages.number.negative;
                return false;
            } else if (!regExpNumber.test(this.baseModel.number)) {
                this.errorLabels.numberError = this.errorMessages.number.format;
                return false;
            } else {
                this.errorLabels.numberError = "";
            }
            return true;
        },
        validateLabel() {
            if (typeof this.baseModel.label == "undefined" || this.baseModel.label == "") {
                this.errorLabels.labelError = this.errorMessages.label.empty;
                return false;
            } else if (!regExpLabel.test(this.baseModel.label)) {
                this.errorLabels.labelError = this.errorMessages.label.format;
                return false;
            } else {
                this.errorLabels.labelError = "";
            }
            return true;
        },
        validatePostalCode() {
            if (typeof this.baseModel.postalCode == "undefined" || this.baseModel.postalCode.length == 0) {
                this.errorLabels.postalCodeError = this.errorMessages.postalCode.empty;
                return false;
            } else if (this.baseModel.postalCode.length != 5) {
                this.errorLabels.postalCodeError = this.errorMessages.postalCode.size;
                return false;
            } else if (!regExpNumber.test(this.baseModel.postalCode)) {
                this.errorLabels.postalCodeError = this.errorMessages.postalCode.format;
                return false;
            } else {
                this.errorLabels.postalCodeError = "";
            }
            return true;
        },
        validateTown() {
            if (typeof this.baseModel.town == "undefined" || this.baseModel.town.length == 0) {
                this.errorLabels.townError = this.errorMessages.town.empty;
                return false;
            } else if (!regExpLabel.test(this.baseModel.town)) {
                this.errorLabels.townError = this.errorMessages.town.format;
                return false;
            } else {
                this.errorLabels.townError = "";
            }
            return true;
        }
    },
    props: ['baseModel', 'readOnly']
}
</script>

<style>

</style>
