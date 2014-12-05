jQuery(document).ready(function($) {
    // FORM ELEMENTS //
    
    function init_form_styling() {
        
        // SELECT //
        
        $("select.faux").each(function() {
            $(this).wrap("<div class='faux_select'></div>");
            $(this).parent().prepend("<span class='selected_text'></span><span class='handle'></span><span class='options'></span>");
            var selected = $(this).children('option:selected').text();
            $(this).parent().children('.selected_text').text(selected);
            var options_container = $(this).parent().children('.options');
            $(this).removeClass('faux');
            $(this).children('option').each(function() {
                var value = $(this).val();
                var text = $(this).text();
                options_container.append('<span data-value="'+value+'">'+text+'</span>');
            });
        });

        // CHECKBOX //

        $("input[type=checkbox].faux").each(function() {
            $(this).wrap("<div class='faux_checkbox'></div>");
            $(this).parent().prepend("<i class='fa fa-check'></i>");
            if ($(this).is(':checked')) {
                $(this).parent().addClass('checked');
            }
            $(this).removeClass('faux');
        });        
    }
    
    init_form_styling();
    
    // SELECT //
    
    $('body').on('click', '.faux_select', function() {
        if ($(this).children('.options').height() == 0) {
            // Open options window
            $(this).children('.options').css({border:'1px solid #E2E2E2'}).animate({height:'150px'}, 100);
            $(this).addClass('open');
            $(this).children('select').focus();
        }
    });

    $('body').on('click', '.faux_select .options span', function() {
        var value = $(this).data('value');
        var text = $(this).text();
        $(this).parents('.faux_select').children('.selected_text').text(text);
        $(this).parents('.faux_select').children('select').val(value);
        $(this).parents('.faux_select').children('select').trigger('change');
        $(this).parents('.faux_select').trigger('click');
    });

    $('body').on('blur', '.faux_select select', function() {
        $(this).parents('.faux_select').children('.options').delay(100).animate({height:'0px'}, 100, function() {
            $(this).css({border:'1px solid #fff'});
        });
        $(this).parents('.faux_select').removeClass('open');
    });
    
    // CHECKBOX //
    
    $('body').on('click', ".faux_checkbox input[type=checkbox]", function() {
        if ($(this).is(':checked')) {
            // Already checked. Uncheck
            $(this).parent().addClass('checked');
            $(this).attr('checked', 'checked');
        } else {
            // Check dat box
            $(this).parent().removeClass('checked');
            $(this).removeAttr('checked');
        }
    });

    $('body').on('click', '.faux_checkbox', function() {
        $(this).children('input[type=checkbox]').trigger('click');
    });
    
    // ABOUT FLICKER //
    
    $(".content-block-link").hover(function() {
        if (!$(this).hasClass('active')) {
            $(".content-block-link.active").removeClass('active');
            $(this).addClass('active');
            
            $(".content-block-image.active").removeClass('active');
            $("#content_block_image_"+$(this).data('id')).addClass('active');
            
            $(".content-block.active").removeClass('active');
            $("#content_block_"+$(this).data('id')).fadeIn(0, function() {
                $(this).addClass('active').removeAttr('style');
            });
        }
    });
    
    $(".toggleform").click(function() {
        if ($("#toggleform").css('display') == 'none') {
            $("#toggleform").fadeIn();
        } else {
            $("#toggleform").fadeOut();
        }
    });
    
    // COUNTER //
    
    if ($("#move_counter").length > 0) {
        var value = $("#move_counter").text();
        var newvalue = '';
        for (var i = 0, len = value.length; i < len; i++) {
            if ($.isNumeric(value[i])) {
                newvalue += "<div class='number'><span class='new'></span><span class='old'>"+value[i]+"</span></div>";
            } else {
                newvalue += "<div>"+value[i]+"</div>";
            }
        };
        $("#move_counter").html(newvalue);
        
        setTimeout('randnum()',5000);
    }
    
    // TESTIMONIALS //
    
    $("#testimonials .logos img").mouseover(function() {
        var tid = $(this).parent().data('id');
        
        if (!$("#testimonials .testimonials > div#testimonial_"+tid).hasClass('active')) {
            $("#testimonials .logos > div.active").removeClass('active');
            $("#testimonials .testimonials > div.active").hide(0, function() {
               $(this).removeAttr('style').removeClass('active');
            });

            $(this).parent().addClass('active');
            $("#testimonials .testimonials > div#testimonial_"+tid).fadeIn('500', function() {
                $(this).removeAttr('style').addClass('active');
                $("#testimonials .testimonials > div.active").not(this).removeClass('active');
            });
        }
    });
    
    // IN THE NEWS //
    
    $("#news-slider ul").bxSlider({
        controls: false,
        auto: false,
        speed: 1000,
        pause: 7000,
        /*onSlideBefore: function(slide, oldIndex, newIndex) {
            console.log(slide.index());
            console.log(oldIndex);
            console.log(newIndex);
            
            if (oldIndex < newIndex) {
                slide.prev().find('.news-block').animateRotate(0, -15, 300, 'swing');
                //slide.prev().find('.news-block').css('transform', 'rotate(-15deg)');
                slide.find('.news-block').animateRotate(0, -15, 300, 'swing');
            } else {
                slide.next().find('.news-block').animateRotate(0, 15, 300, 'swing');
                //slide.prev().find('.news-block').css('transform', 'rotate(-15deg)');
                slide.find('.news-block').animateRotate(0, 15, 300, 'swing');
            }
        },
        onSlideAfter: function (slide, oldIndex, newIndex) {
            if (oldIndex < newIndex) {
                slide.prev().find('.news-block').animateRotate(-15, 0, 100, 'swing');
                //slide.prev().find('.news-block').css('transform', 'rotate(-15deg)');
                slide.find('.news-block').animateRotate(-15, 0, 100, 'swing');
            } else {
                slide.next().find('.news-block').animateRotate(15, 0, 100, 'swing');
                //slide.prev().find('.news-block').css('transform', 'rotate(-15deg)');
                slide.find('.news-block').animateRotate(15, 0, 100, 'swing');
            }
        },
        onSlideNext: function(e, o, n) {
            if (e.index() === 1) {
                e.parent().children('li:first-child').find('.news-block').animateRotate(0, -15, 300, 'swing');
                e.parent().children('li:last-child').find('.news-block').animateRotate(0, -15, 300, 'swing');
                e.prev().find('.news-block').animateRotate(0, -15, 300, 'swing');
                e.find('.news-block').animateRotate(0, -15, 300, 'swing');
            } else {
                e.prev().find('.news-block').animateRotate(0, -15, 300, 'swing');
                e.find('.news-block').animateRotate(0, -15, 300, 'swing');
            }
        },
        onSlidePrev: function(e, o, n) {
            e.next().find('.news-block').animateRotate(0, 15, 300, 'swing');
            e.find('.news-block').animateRotate(0, 15, 300, 'swing');
//            var count = e.parent().children('li').length;
//            if (e.index() === count-2) {
//                e.parent().children('li:first-child').find('.slider-info').css('right', '120%').animate({'right':'20%'}, 2000);
//            }
//            e.find('.slider-info').css('right', '120%').animate({'right':'20%'}, 2000);
        }*/
    });
    
    $.fn.animateRotate = function(startangle, endangle, duration, easing, complete) {
        var args = $.speed(duration, easing, complete);
        var step = args.step;
        return this.each(function(i, e) {
            args.step = function(now) {
                $.style(e, 'transform', 'rotate(' + now + 'deg)');
                if (step) return step.apply(this, arguments);
            };

            $({deg: startangle}).animate({deg: endangle}, args);
        });
    };
});

function randnum() {
    var numbers = jQuery("#move_counter").children('.number').length;
    var actual = '';
    for (var c=0;c<parseInt(numbers);c++) {
        actual += jQuery("#move_counter").children('.number').eq(c).children('.old').text();
    }
    //console.log("Actual: "+actual);
    var highest = parseInt(actual) + 100;
    //console.log("Highest: "+highest);
    var adjustedHigh = (parseFloat(highest) - parseFloat(actual)) + 1;
    var randnum = Math.ceil(Math.random() * parseInt(adjustedHigh)) + parseFloat(actual);
//    console.log("New: "+randnum);
//    console.log(randnum.toString().length);

    randnum = randnum.toString();
    //console.log(randnum);
    for (var c=0;c<randnum.length;c++) {
        var current = jQuery("#move_counter").children('.number').eq(c).children('.old').text();
        var newnum = randnum[c];
        if (parseInt(current) !== parseInt(newnum)) {
            //console.log("Changing: "+c+" to "+newnum);
            jQuery("#move_counter").children('.number').eq(c).children('.new').text(parseInt(newnum)).addClass('move');
            jQuery("#move_counter").children('.number').eq(c).children('.old').addClass('move');
        }
    }
    
    var speed = {};
    speed[10] = 4000 / 40;
    speed[9] = 4000 / 20;
    speed[8] = 4000 / 15;
    speed[6] = 4000 / 10;
    speed[5] = 4000 / 9;
    speed[4] = 4000 / 8;
    speed[2] = 4000 / 7;
    speed[1] = 4000 / 6;
    speed[0] = 4000 / 5;
    
    jQuery("#move_counter").children('.number').children('.old.move').each(function() {
        var ind = jQuery(this).parent().index();
//        console.log("old index: "+ind);
//        var speed = 4000 / (parseInt(ind) * 1.3);
        //console.log("speed: "+speed);
        jQuery(this).animate({top: '58px'}, speed[ind], function() {
            jQuery(this).removeClass('old').removeClass('move').addClass('new').removeAttr('style');
        });
    });
    jQuery("#move_counter").children('.number').children('.new.move').each(function() {
        var ind = jQuery(this).parent().index();
//        console.log("new index: "+ind);
//        var speed = 4000 / (parseInt(ind) * 1.3);
        //console.log("speed: "+speed);
        jQuery(this).animate({top: '0px'}, speed[ind], function() {
            jQuery(this).removeClass('new').removeClass('move').addClass('old').removeAttr('style');
        });
    });
    
    setTimeout('randnum()',5000);
}