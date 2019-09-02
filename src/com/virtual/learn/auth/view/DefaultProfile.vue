<template>
    <div style="height: 100%">
        <div id="left-menu" class="menu-left"></div>
        <div id="top-menu" class="top-menu">
            <div class="logo"></div>
            <div class="menu-title">
                <label>Learn in Virtual environment</label>
            </div>
        </div>
        <router-view></router-view>
    </div>
</template>

<script>
    import axios from 'axios'
    
    export default {
        name: 'Profile',
        data() {
            return {
                
            }
        }, 
        created() {
            this.$router.push({name: "login"});
        }
    }
</script>

<style>
</style>