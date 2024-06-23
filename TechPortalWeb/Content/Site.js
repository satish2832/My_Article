document.getElementById('btnBookDemo').addEventListener('click', function () {
    var divBookDemo = new bootstrap.Modal(document.getElementById('divBookDemo'), {
        backdrop: 'static',
        keyboard: false
    });
    divBookDemo.show();
});