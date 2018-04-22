var contentWayPoint = function() {
    var i = 0;
    $(".probootstrap-animate").waypoint(function(direction) {

            if (direction === "down" && !$(this.element).hasClass("probootstrap-animated")) {

                i++;

                $(this.element).addClass("item-animate");
                setTimeout(function() {

                        $("body .probootstrap-animate.item-animate").each(function(k) {
                            var el = $(this);
                            setTimeout(function() {
                                    var effect = el.data("animate-effect");
                                    if (effect === "fadeIn") {
                                        el.addClass("fadeIn probootstrap-animated");
                                    } else if (effect === "fadeInLeft") {
                                        el.addClass("fadeInLeft probootstrap-animated");
                                    } else if (effect === "fadeInRight") {
                                        el.addClass("fadeInRight probootstrap-animated");
                                    } else {
                                        el.addClass("fadeInUp probootstrap-animated");
                                    }
                                    el.removeClass("item-animate");
                                },
                                k * 50,
                                "easeInOutExpo");
                        });

                    },
                    100);

            }

        },
        { offset: "95%" });
};

var getCategories = function() {
    $.ajax({
        url: "/Categories/GetCategories",
        method: "GET"
    }).done(function(data) {
        var container = $("#categories");
        var print = "";
        for (var i = 0; i < data.length; i++) {
            var category = data[i];
            print += '<li><a href="/Categories/Detail/' +
                category.Id +
                '#categories"><i class="fa ' +
                category.Icon +
                '" area-hidden="true"></i><span>' +
                category.Name +
                "</span></a></li>";
        }
        container.find("ul").html(print);
    }).fail(function() {
        toastr.error("Something unexpected happen!");
    });
};

var getDishes = function() {
    var currentPage = 0;
    var totalPage = 0;
    var canLoadMore = true;
    var loadDone = true;

    var loadDishes = function() {
        loadDone = false;
        var $container = $(".card-columns");
        var page = currentPage + 1;
        var urlAjax = "";
        if (!$("#dishes").attr("data-category-id")) {
            urlAjax = "/Dishes/Page/" + page;
        } else {
            var id = parseInt($("#dishes").attr("data-category-id"));
            urlAjax = "/Dishes/Category/" + id + "/Page/" + page;
        }
        $.ajax({
            url: urlAjax,
            method: "GET"
        }).done(function(data) {
            var dishes = data.Data;
            currentPage = parseInt(data.CurrentPage);
            totalPage = parseInt(data.TotalPage);
            if (page >= totalPage) canLoadMore = false;
            for (item in dishes) {
                var dish = dishes[item];
                var imageUrl = data.Server + dish.ImageUrl;
                $container.append(
                    '<div class="card"><a href="#"><img class="card-img-top probootstrap-animate" src="' +
                    imageUrl +
                    '" alt="' +
                    dish.Name +
                    '" data-animate-effect="fadeIn"/>' +
                    '<div class="price-tag">' +
                    dish.Price +
                    " BND</div>" +
                    "</a>" +
                    '<div class="card-block">' +
                    '<h4 class="card-title">' +
                    dish.Name +
                    "</h4>" +
                    "</div>" +
                    "</div>");
            }

            $container.imagesLoaded()
                .progress(contentWayPoint())
                .done(function() {
                    $container.find(".card").addClass("img-loaded");
                    loadDone = true;
                });
        }).fail(function() {
            toastr.error("Something unexpected happen!");
        });
    };

    loadDishes();

    $(window).scroll(function() {
        if (canLoadMore) {
            if ($(window).scrollTop() + $(window).height() > $(document).height() - 50) {
                if (loadDone)
                    loadDishes();
            }
        }
    });
};

var getRestaurants = function() {
    var currentPage = 0;
    var totalPage = 0;
    var canLoadMore = true;
    var loadDone = true;

    var loadRestaurants = function() {
        loadDone = false;
        var $container = $("#restaurant");
        var page = currentPage + 1;
        $.ajax({
            url: "/Restaurants/Page/" + page,
            method: "GET"
        }).done(function(data) {
            var restaurants = data.Data;
            currentPage = parseInt(data.CurrentPage);
            totalPage = parseInt(data.TotalPage);
            if (currentPage >= totalPage) canLoadMore = false;
            var dataPrint = '<div class="row">';
            for (var i = 0; i < restaurants.length; i++) {
                var restaurant = restaurants[i];
                var imageUrl = data.Server + restaurant.ImageUrl;
                if (i === 0 || i === 3) dataPrint += '<div class="card-deck">';
                dataPrint += '<div class="card">' +
                    '<img src="' +
                    imageUrl +
                    '" class="card-img-top img-responsive probootstrap-animate" data-animate-effect="fadeIn"/>' +
                    '<div class="card-block">' +
                    '<h4 class="card-title">' +
                    restaurant.Name +
                    "</h4>" +
                    "</div>" +
                    '<ul class="list-group list-group-flush">' +
                    '<li class="list-group-item"><i class="fa fa-map-marker" aria-hidden="true"></i> <span>Address:</span> ' +
                    restaurant.Address +
                    "</li>" +
                    '<li class="list-group-item"><i class="fa fa-mobile" aria-hidden="true"></i> <span>Phone:</span> ' +
                    restaurant.PhoneNumber +
                    "</li>" +
                    '<li class="list-group-item"><i class="fa fa-clock-o" aria-hidden="true"></i> <span>Hours working:</span> ' +
                    restaurant.OpenTime.Hours +
                    ":" +
                    restaurant.OpenTime.Minutes +
                    " - " +
                    restaurant.CloseTime.Hours +
                    ":" +
                    restaurant.CloseTime.Minutes +
                    "</li>" +
                    '<li class="list-group-item"><i class="fa fa-cutlery" aria-hidden="true"></i> <span>Halal:</span> ' +
                    (restaurant.IsHalal ===
                        true
                        ? " Yes "
                        : " No ") +
                    "</li>" +
                    '<li class="list-group-item"><i class="fa fa-glass" aria-hidden="true"></i> <span>Dishes:</span> ' +
                    restaurant.NumberOfDishes +
                    "</li>" +
                    "</ul>" +
                    '<div class="card-block">' +
                    '<a href="/Restaurants/Detail/' +
                    restaurant.Id +
                    '" class="btn btn-primary btn-success">Read More</a>' +
                    "</div>" +
                    "</div>";
                if (i === 3 || i === 6) {
                    dataPrint += "</div>";
                }
            }
            dataPrint += "</div>";
            $container.append(dataPrint);
            $container.imagesLoaded()
                .progress(contentWayPoint())
                .done(function() {
                    $container.find(".card").addClass("img-loaded");
                    loadDone = true;
                });
        }).fail(function() {
            toastr.error("Something unexpected happen!");
        });
    };
    loadRestaurants();

    $(window).scroll(function() {
        if (canLoadMore) {
            if ($(window).scrollTop() + $(window).height() > $(document).height() - 50) {
                if (loadDone)
                    loadRestaurants();
            }
        }
    });
};

$(document).ready(function() {
    $(".btn-toggle-fullwidth").on("click",
        function() {
            if (!$("body").hasClass("layout-fullwidth")) {
                $("body").addClass("layout-fullwidth");

            } else {
                $("body").removeClass("layout-fullwidth");
                $("body").removeClass("layout-default"); // also remove default behaviour if set
            }

            $(this).find(".lnr").toggleClass("lnr-arrow-left-circle lnr-arrow-right-circle");

            if ($(window).innerWidth() < 1025) {
                if (!$("body").hasClass("offcanvas-active")) {
                    $("body").addClass("offcanvas-active");
                } else {
                    $("body").removeClass("offcanvas-active");
                }
            }
        });

    $(window).on("load",
        function() {
            if ($(window).innerWidth() < 1025) {
                $(".btn-toggle-fullwidth").find(".icon-arrows")
                    .removeClass("icon-arrows-move-left")
                    .addClass("icon-arrows-move-right");
            }

            // adjust right sidebar top position
            $(".right-sidebar").css("top", $(".navbar").innerHeight());

            // if page has content-menu, set top padding of main-content
            if ($(".has-content-menu").length > 0) {
                $(".navbar + .main-content").css("padding-top", $(".navbar").innerHeight());
            }

            // for shorter main content
            if ($(".main").height() < $("#sidebar-nav").height()) {
                $(".main").css("min-height", $("#sidebar-nav").height());
            }
        });


    $('.sidebar a[data-toggle="collapse"]').on("click",
        function() {
            if ($(this).hasClass("collapsed")) {
                $(this).addClass("active");
            } else {
                $(this).removeClass("active");
            }
        });
});