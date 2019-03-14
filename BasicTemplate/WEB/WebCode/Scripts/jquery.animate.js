(function ($) {
    $.fn.extend({
        animateCss: function (animationName) {
            var animationEnd = 'webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend';
            $(this).addClass('animated ' + animationName).one(animationEnd, function () {
                $(this).removeClass('animated ' + animationName);
            });
        }
    });

    $.animateOption = {
        bounce: {
            in: "bounceIn",
            out: "bounceOut"
        },
        bounceDown: {
            in: "bounceInDown",
            out: "bounceOutDown"
        },
        bounceLeft: {
            in: "bounceInLeft",
            out: "bounceOutLeft"
        },
        bounceRight: {
            in: "bounceInRight",
            out: "bounceOutRight"
        },
        bounceUp: {
            in: "bounceInUp",
            out: "bounceOutUp"
        },

        fade: {
            in: "fadeIn",
            out: "fadeOut"
        },
        fadeDown: {
            in: "fadeInDown",
            out: "fadeOutDown"
        },
        fadeDownBig: {
            in: "fadeInDownBig",
            out: "fadeOutDownBig"
        },
        fadeLeft: {
            in: "fadeInLeft",
            out: "fadeOutLeft"
        },
        fadeLeftBig: {
            in: "fadeInLeftBig",
            out: "fadeOutLeftBig"
        },
        fadeRight: {
            in: "fadeInRight",
            out: "fadeOutRight"
        },
        fadeRightBig: {
            in: "fadeInRight",
            out: "fadeOutRightBig"
        },
        fadeUp: {
            in: "fadeInUp",
            out: "fadeOutUp"
        },
        fadeUpBig: {
            in: "fadeInUpBig",
            out: "fadeOutUpBig"
        },

        flipX: {
            in: "flipInX",
            out: "flipOutX"
        },
        flipY: {
            in: "flipInY",
            out: "flipOutY"
        },

        lightSpeed: {
            in: "lightSpeedIn",
            out: "lightSpeedOut"
        },

        rotate: {
            in: "rotateIn",
            out: "rotateOut"
        },
        rotateInDownLeft: {
            in: "rotateInDownLeft",
            out: "rotateOutDownLeft"
        },
        rotateInDownRight: {
            in: "rotateInDownRight",
            out: "rotateOutDownRight"
        },
        rotateInUpLeft: {
            in: "rotateInUpLeft",
            out: "rotateOutUpLeft"
        },
        rotateInUpRight: {
            in: "rotateInUpRight",
            out: "rotateOutUpRight"
        },

        slideUp: {
            in: "slideInUp",
            out: "slideOutUp"
        },
        slideDown: {
            in: "slideInDown",
            out: "slideOutDown"
        },
        slideLeft: {
            in: "slideInLeft",
            out: "slideOutLeft"
        },
        slideRight: {
            in: "slideInRight",
            out: "slideOutRight"
        },

        zoom: {
            in: "zoomIn",
            out: "zoomOut"
        },
        zoomDown: {
            in: "zoomInDown",
            out: "zoomOutDown"
        },
        zoomLeft: {
            in: "zoomInLeft",
            out: "zoomOutLeft"
        },
        zoomRight: {
            in: "zoomInRight",
            out: "zoomOutRight"
        },
        zoomUp: {
            in: "zoomInUp",
            out: "zoomOutUp"
        },

        //special: {
        //    in: "hinge",
        //    out: "jackInTheBox"
        //},

        //roll: {
        //    in: "rollIn",
        //    out: "rollOut"
        //}




    };

    $.animateList = [];

    for (var i in $.animateOption) {
        $.animateList.push($.animateOption[i]);
    }

})(jQuery);