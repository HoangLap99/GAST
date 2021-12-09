// for menu mobile and desktop ---------------------------------------------------------->
$(function () {
  $("#main-menu").smartmenus({
    mainMenuSubOffsetX: -1,
    mainMenuSubOffsetY: 4,
    subMenusSubOffsetX: 6,
    subMenusSubOffsetY: -6,
  });
});

// SmartMenus CSS animated sub menus - toggle animation classes on sub menus show/hide
$(function () {
  $("#main-menu")
    .bind({
      "show.smapi": function (e, menu) {
        $(menu).removeClass("hide-animation").addClass("show-animation");
      },
      "hide.smapi": function (e, menu) {
        $(menu).removeClass("show-animation").addClass("hide-animation");
      },
    })
    .on(
      "animationend webkitAnimationEnd oanimationend MSAnimationEnd",
      "ul",
      function (e) {
        $(this).removeClass("show-animation hide-animation");
        e.stopPropagation();
      }
    );
});

// show and hide menu mobile
$(function () {
  $("#show-menu-mobile").click(function () {
    if ($("#main-menu").css("left") == "-2000px") {
      $("#main-menu").css("left", "0px");
      $("#show-menu-mobile").css("color", "#000");
      $("#logo-green").css("display", "block");
      $("#logo-white").css("display", "none");
      $(".header-fixed").css("background-color", "#fff");
    } else {
      $("#main-menu").css("left", "-2000px");
      $("#show-menu-mobile").css("color", "#fff");
      $("#logo-green").css("display", "none");
      $("#logo-white").css("display", "block");
      $(".header-fixed").css("background-color", "transparent");
    }
  });
});

// back to top
$(function () {
  $("#to-top").on("click", function () {
    $("html, body").animate(
      {
        scrollTop: 0,
      },
      200
    );
  });
});

// responsive when change display

$(window).resize(function () {
  let width = $(window).width();
  // for guest
  if (width > 1023) {
    $(".guest__block").addClass("guest-position");
  } else {
    $(".guest__block").removeClass("guest-position");
  }

  // for event
  if (width < 414) {
    $(".event-responsive").addClass("event-text");
  } else {
    $("p.event").removeClass("event-text");
  }
});

// validate ---------------------------------------------------------->
// validate email in footer
$("#validate").validate({
  rules: {
    email: {
      required: true,
      email: true,
    },
  },
  messages: {
    email: {
      required: "Please enter email",
      email: "Email not true of format",
    },
  },
});

// daterangepicker -------------------------------------->

$(function () {
    $('input[name="fromDate"]').daterangepicker({
        singleDatePicker: true,
        autoUpdateInput: false,
    });
});
$('input[name="fromDate"]').on("apply.daterangepicker", function (ev, picker) {
    $(this).val(picker.startDate.format("DD/MM/YYYY"));
});

$(function () {
    $('input[name="toDate"]').daterangepicker({
        singleDatePicker: true,
        autoUpdateInput: false,
    });
});
$('input[name="toDate"]').on("apply.daterangepicker", function (ev, picker) {
    $(this).val(picker.startDate.format("DD/MM/YYYY"));
});
$(function () {
  $('input[name="checkIn"]').daterangepicker({
    singleDatePicker: true,
    autoUpdateInput: false,
  });
});
$('input[name="checkIn"]').on("apply.daterangepicker", function (ev, picker) {
  $(this).val(picker.startDate.format("DD/MM/YYYY"));
});

// scroll page
var lastScrollTop = 0;
$(document).ready(function () {
  $(window).scroll(function (event) {
    let pos_body = $("html,body").scrollTop();
    if (pos_body > 50) {
      $("#to-top").css("display", "block");
    } else {
      $("#to-top").css("display", "none");
    }

    var st = $(this).scrollTop();
    if (st > lastScrollTop || st == lastScrollTop) {
      $(".header-fixed").removeClass("fixed-menu");
    } else {
      if (pos_body > 50) {
        $(".header-fixed").addClass("fixed-menu");
        $("#main-menu  a").css("color", "#000");
        $(".header-fixed").css("background-color", "#fff");
        $("#logo-green").css("display", "block");
        $("#logo-white").css("display", "none");
        $("#show-menu-mobile").css("color", "#000");
      } else {
        $(".header-fixed").removeClass("fixed-menu");
        $(".header-fixed").css("background-color", "transparent");
        $("#main-menu  a").css("color", "#fff");
        $("#logo-green").css("display", "none");
        $("#logo-white").css("display", "block");
        $("#show-menu-mobile").css("color", "#fff");
      }
    }
    lastScrollTop = st;
  });
});
$(".header-fixed").hover(
  function () {
    $(".header-fixed").css("background-color", "#fff");
    $("#main-menu  a").css("color", "#000");
    $("#logo-green").css("display", "block");
    $("#logo-white").css("display", "none");
  },
  function () {
    $(".header-fixed").css("background-color", "transparent");
    $("#main-menu  a").css("color", "#fff");
    $("#logo-green").css("display", "none");
    $("#logo-white").css("display", "block");
  }
);

// counter in meeting rooms page
$(function () {
  $(".count").each(function () {
    $(this)
      .prop("Counter", 0)
      .animate(
        {
          Counter: $(this).text(),
        },
        {
          duration: 4000,
          easing: "swing",
          step: function (now) {
            $(this).text(Math.ceil(now));
          },
        }
      );
  });
});
