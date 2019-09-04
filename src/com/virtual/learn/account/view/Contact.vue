<template>
    <div>
        <div id="updateContact" v-if="readOnly === false">
            <div class="smallInlineField">
                <input type="text" name="phone" v-model="baseModel.phone" placeholder="PhoneNumber" v-on:blur="validatePhone()" />
                <p v-if="errorLabels.phoneError.length > 0" class="error">{{errorLabels.phoneError}}</p><br v-else/>
            </div>
            <div class="bigInlineField">
                <input type="text" name="mail" v-model="baseModel.mail" placeholder="MailAdress" v-on:blur="validateMail()" />
                <p v-if="errorLabels.mailError.length > 0" class="error">{{errorLabels.mailError}}</p><br v-else/>
            </div>
            <input type="text" name="web" v-model="baseModel.web" placeholder="Web" />
        </div>

        <div id="readContact" v-if="readOnly === true">
            <label>{{baseModel.phone}}</label><br v-if="baseModel.phone != ''"/>
            <label>{{baseModel.mail}}</label><br v-if="baseModel.mail != ''"/>
            <label>{{baseModel.web}}</label>
        </div>
    </div>
</template>

<script>
export default {
    name: 'Contact',
    data() {
        return {
            errorLabels: {
                mailError: "",
                phoneError: ""
            },
            errorMessages: {
                mail: {
                    empty: "L'adresse mail est obligatoire.",
                    format: "L'adresse mail ne correspond pas au format attendu (exemple : contact@agence.fr)."
                },
                phone: {
                    format: "Ce champ n'accepte que des numéros.",
                    size: "Un numéro de téléphone fait une longueur de 10 chiffres, ni plus ni moins."
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
            if (!this.validateMail()) {
                valid = false;   
            }
            if (!this.validatePhone()) {
                valid = false;   
            }
            return valid;
        },
        validateMail() {
            // Cannot use the same constructor as the author, due to the complexity of the regexp (bad interpretation by the loader)
            var regexp = /^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}$/;

            if (typeof this.baseModel.mail == "undefined" || this.baseModel.mail.length == 0) {
                this.errorLabels.mailError = this.errorMessages.mail.empty;
                    return false;
            } else if (!regexp.test(this.baseModel.mail)) {
                // Caution, with the difference in construction of the regexp, the result of .test is inverted
                this.errorLabels.mailError = this.errorMessages.mail.format;
                    return false;
            } else {
                this.errorLabels.mailError = "";
            }
            return true;
            
        },
        validatePhone() {
            var regexp = /^[0-9]+$/;
            if (typeof this.baseModel.phone != "undefined" && this.baseModel.phone.length > 0) {
                if (!regexp.test(this.baseModel.phone)) {
                    this.errorLabels.phoneError = this.errorMessages.phone.format;
                    return false;
                } else if(this.baseModel.phone.length != 10) {
                    this.errorLabels.phoneError = this.errorMessages.phone.size;
                    return false;
                } else {
                    this.errorLabels.phoneError = "";
                }
            }
            return true;
        }
    },
    props: ['baseModel', 'readOnly']
}
</script>

<style>

</style>
