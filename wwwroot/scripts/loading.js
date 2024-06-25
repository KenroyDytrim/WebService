document.body.onload = function () {
    setTimeout(function () {
        var loader = document.getElementById('page-loader');
        var loader2 = document.getElementById('load-scr');
        if (!loader.classList.contains('done')) {
            loader.classList.add('done');
            loader2.classList.add('done');
        }
    }, 5000)
   
}