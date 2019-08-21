<template>
    <div id="page">
        <div id="searchCriterias">
            <input type="text" name="label" v-model="searchCriterias.label" placeholder="Label" />
            <button type="button" v-on:click="search()">Rechercher</button>
            <b-button v-b-toggle.advanced class="m-1" >Recherche avancée</b-button>
            <b-collapse id="advanced" accordion="advanced-search" class="mt-2">
                 <select v-on:change="sorting($event)">
                    <option value="default" disabled selected>Durée</option>
                    <option value="lessLengthFirst">Par ordre croissant</option>
                    <option value="moreLengthFirst">Par ordre décroissant</option>
                </select> 
                <select v-on:change="sorting($event)">
                    <option value="default" disabled selected>Crédits</option>
                    <option value="lessCreditsFirst">Par ordre croissant</option>
                    <option value="moreCreditsFirst">Par ordre décroissant</option>
                </select> 
            </b-collapse>
        </div>
        <div id="modules">
            <div class="module" v-for="module in baseModel.modules" v-bind:key="module.moduleId" >
                <div>
                    <b>{{module.label}}</b><br/>
                    Durée : {{module.length}} min<br/>
                    Type : {{module.type}}<br/>
                    Nombre de crédits : {{module.nbCredits}}
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    import axios from 'axios'
    export default {
        name: 'Catalog',
        data() {
            return {
                searchCriterias: {
                    label: "",
                    type: "",
                    length: "",
                    nbCredits: "",
                    moduleId: "",
                    userId: ""
                },
                baseModel: {
                    modules: []
                }
            };
        },
        created() {
            // mandatory
            // the instance that 'this' reference in axios doesn't point to the vue instance
            console.log("ok modules");
            var instance = this;
            axios({
                method: 'post',
                url: "/Module/SearchModules",
                data: instance.searchCriterias
            }).then(function(response) {
                // we dynamically set the size of the array so the reactive side can still be engaged
                instance.baseModel.modules.splice(response.data.length);
                instance.baseModel.modules = Object.assign(instance.baseModel.modules, response.data);
            }).catch(function (error) {
                console.log(error);
            });
        },
        methods: {
            search() {
                // mandatory
                // the instance that 'this' reference in axios doesn't point to the vue instance
                var instance = this;
                axios({
                    method: 'post',
                    url: "/Module/SearchModules",
                    data: instance.searchCriterias
                }).then(function(response) {
                    // we dynamically set the size of the array so the reactive side can still be engaged
                    instance.baseModel.modules.splice(response.data.length);
                    instance.baseModel.modules = Object.assign(instance.baseModel.modules, response.data);
                }).catch(function (error) {
                    console.log(error);
                });
            },
            sorting(event) {
                var sortingType = event.target.value;
                function compare(a, b) {
                    if ("lessLengthFirst" == sortingType) {
                        if (a.length < b.length)
                            return -1;
                        if (a.length > b.length)
                            return 1;
                        return 0;
                    } else if ("moreLengthFirst" == sortingType) {
                        if (a.length < b.length)
                            return 1;
                        if (a.length > b.length)
                            return -1;
                        return 0;
                    } else if ("lessCreditsFirst" == sortingType) {
                        if (a.nbCredits < b.nbCredits)
                            return -1;
                        if (a.nbCredits > b.nbCredits)
                            return 1;
                        return 0;
                    } else if ("moreCreditsFirst" == sortingType) {
                        if (a.nbCredits < b.nbCredits)
                            return 1;
                        if (a.nbCredits > b.nbCredits)
                            return -1;
                        return 0;
                    }
                }

                return this.baseModel.modules.sort(compare);
            }
        }
    }
</script>

<style scoped>
    .module {
    border: solid black 1px;
    width: 200px;
    margin: 20px;
    padding: 20px;
}
</style>