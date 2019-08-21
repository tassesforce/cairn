<template>
    <div>
        <h2>Responsable</h2>
        <div id="updateResponsible" v-if="readOnly === false">
            <span class="mandatory">*</span><input type="text" name="firstName" v-model="baseModel.firstName" placeholder="FirstName" v-on:blur="validateFirstName()" />
            <p v-if="errorLabels.firstNameError.length > 0" class="error">{{errorLabels.firstNameError}}</p><br v-else/>
            <span class="mandatory">*</span><input type="text" name="lastName" v-model="baseModel.lastName" placeholder="LastName" v-on:blur="validateLastName()" />
            <p v-if="errorLabels.lastNameError.length > 0" class="error">{{errorLabels.lastNameError}}</p><br v-else/>
            <input type="text" name="position" v-model="baseModel.position" placeholder="Position" />
        </div>

        <div id="readResponsible" v-if="readOnly === true">
            <p>{{baseModel.firstName}}</p>
            <p>{{baseModel.lastName}}</p>
            <p>{{baseModel.position}}</p>
        </div>
    </div>
</template>

<script>
var regexp = /^[a-zA-Z- ]+$/;

export default {
    name: 'Responsible',
    data() {
        return {
            errorLabels: {
                firstNameError: "",
                lastNameError: ""
            },
            errorMessages: {
                first: {
                    empty: "Le prénom du responsable est obligatoire.",
                    format: "Désolé, mais le nom que vous avez saisi ne semble pas correspondre au patron standard (numéros et caractères spéciaux interdits)."
                },
                last: {
                    empty: "Le nom de famille du responsable est obligatoire.",
                    format: "Désolé, mais le nom que vous avez saisi ne semble pas correspondre au patron standard (numéros et caractères spéciaux interdits)."
                }
            }
        }
    },
    methods: {
        fetchDatas() {
            return this.baseModel;
        },
        validateForm() {
            var valid = true;
            if (!this.validateFirstName()) {
                valid = false;   
            }
            if (!this.validateLastName()) {
                valid = false;   
            }
            return valid;
        },
        validateFirstName() {
            if (typeof this.baseModel.firstName == "undefined" || this.baseModel.firstName.length == 0) {
                this.errorLabels.firstNameError = this.errorMessages.first.empty;
                return false;
            } else if (!regexp.test(this.baseModel.firstName)) {
                this.errorLabels.firstNameError = this.errorMessages.first.format;
                return false;
            } else {
                this.errorLabels.firstNameError = "";
            }
            return true;
        },
        validateLastName() {
            if (typeof this.baseModel.lastName == "undefined" || this.baseModel.lastName.length == 0) {
                this.errorLabels.lastNameError = this.errorMessages.last.empty;
                return false;
            } else if (!regexp.test(this.baseModel.lastName)) {
                this.errorLabels.lastNameError = this.errorMessages.last.format;
                return false;
            } else {
                this.errorLabels.lastNameError = "";
            }
            return true;
        }
    },
    props: ['baseModel', 'readOnly']
}
</script>

<style>

</style>
