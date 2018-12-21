//Provides toggle functionality for admin menu dropdown

$(function () {
    $('[data-admin-menu]').hover(function () {
        $('[data-admin-menu]').toggleClass('open');
    })
});