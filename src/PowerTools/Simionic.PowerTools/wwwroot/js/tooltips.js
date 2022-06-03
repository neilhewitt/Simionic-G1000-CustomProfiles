function addTooltips() {
    $('[data-toggle="tooltip"]').tooltip({
        trigger: 'hover',
        delay: { show: 1000, hide: 1000 }
    });
    $('[data-toggle="tooltip"]').on('mouseleave', function () {
        $(this).tooltip('hide');
    });
    $('[data-toggle="tooltip"]').on('click', function () {
        $(this).tooltip('dispose');
    });
}