<template>
    <div>
        <h2>Caisse de retraite</h2>
        <div id="updateFund" v-if="readOnly === false">
            <span class="mandatory">*</span><input type="text" name="name" v-model="baseModel.name" placeholder="Raison sociale" v-on:blur="validateName()"/>
            <p v-if="errorLabels.nameError.length > 0" class="error">{{errorLabels.nameError}}</p><br v-else/>
            <div id="fundAdress">
                <div><adress ref="adressMarkdown" :baseModel="baseModel.adress" :readOnly="readOnly"></adress></div>
            </div>
        </div>

        <div id="readFund" v-if="readOnly === true">
            <p>{{baseModel.name}}</p>
            <div id="fundAdress">
                <div><adress :baseModel="baseModel.adress" :readOnly="readOnly"></adress></div>
            </div>
        </div>
    </div>
</template>

<script> 
    import adress from '../Adress.vue'
    export default {
        name: 'Fund',
        data() {
            return {
                errorLabels: {
                    nameError: ""
                },
                errorMessages: {
                    name: {
                        empty: "Le nom est obligatoire.",
                    }
                }
            };
        },
        methods: {
            validateForm() {
                var valid = true;
                if (!this.validateName()) {
                    valid = false;
                }
                if (!this.$refs.adressMarkdown.validateForm()) {
                    valid = false;
                }
                return valid;
            },
            validateName() {
                if (typeof this.baseModel.name == "undefined" || this.baseModel.name.length == 0) {
                    this.errorLabels.nameError = this.errorMessages.name.empty;
                    return false;
                } else {
                    this.errorLabels.nameError = "";
                }
                return true;
            }
        },
        components: {
            adress: adress
        },
        props: ['baseModel', 'readOnly']
    }
</script>

<style >

</style>