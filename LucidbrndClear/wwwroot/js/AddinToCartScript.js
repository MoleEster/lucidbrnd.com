jQuery(document).ready(function () {
    var label = $(".added-to-cart");
    if (label != null)
    {
        $('.added-to-cart').show(1000, function () {
            setTimeout(function () {
                $('.added-to-cart').fadeOut(500);
            }, 2000);
        });
    }
});