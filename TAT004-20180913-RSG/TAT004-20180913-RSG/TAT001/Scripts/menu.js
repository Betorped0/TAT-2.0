﻿
var elem = document.querySelector('.collapsible');
var options = [];
var instance = M.Collapsible.init(elem, options);

function abrir(controler) {
    //var c = document.getElementById("container").style.paddingLeft = '300px';
    var c = document.querySelectorAll("div.container");
    for (var i = 0; i < c.length; i++) {
        c[i].style.paddingLeft = '300px';
    }
    document.getElementById('slide-out').style.transform = 'translateX(0%)';
    sessionStorage.setItem('menu.hide', 'false');
    document.getElementById('div-menu').style.width = '320px';
}
function cerrar() {
    //var c = document.getElementById("container").style.paddingLeft = '0px';
    var c = document.querySelectorAll("div.container");
    for (var i = 0; i < c.length; i++) {
        c[i].style.paddingLeft = '0px';
    }
    document.getElementById('slide-out').style.transform = 'translateX(-105%)';
    sessionStorage.setItem('menu.hide', 'true');
    document.getElementById('div-menu').style.width = '100px';
}
function getCookie(cname) {
    return sessionStorage.getItem(cname);
} 