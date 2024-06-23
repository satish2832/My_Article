document.getElementById('btnBookDemo').addEventListener('click', function () {
    var divBookDemo = new bootstrap.Modal(document.getElementById('divBookDemo'), {
        backdrop: 'static',
        keyboard: false
    });
    divBookDemo.show();
});

$('div[data-course-details-url]').on('click', function () {
    var url = $(this).attr("data-course-details-url");
    document.location.href = "/course/" + url;
});