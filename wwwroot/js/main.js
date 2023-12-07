(function ($) {

	"use strict";
		
	// Sticky nav
	var $headerStick = $('.Sticky');
	$(window).on("scroll", function () {
		if ($(this).scrollTop() > 80) {
			$headerStick.addClass("sticky_element");
		} else {
			$headerStick.removeClass("sticky_element");
		};
	});

	// Menu Categories
	$(window).resize(function () {
		if ($(window).width() >= 768) {
			$('a[href="#"]').on('click', function (e) {
				e.preventDefault();
			});
			$('.categories').addClass('menu');
			$('.menu ul > li').on('mouseover', function (e) {
				$(this).find("ul:first").show();
				$(this).find('> span a').addClass('active');
			}).on('mouseout', function (e) {
				$(this).find("ul:first").hide();
				$(this).find('> span a').removeClass('active');
			});
			$('.menu ul li li').on('mouseover', function (e) {
				if ($(this).has('ul').length) {
					$(this).parent().addClass('expanded');
				}
				$('.menu ul:first', this).parent().find('> span a').addClass('active');
				$('.menu ul:first', this).show();
			}).on('mouseout', function (e) {
				$(this).parent().removeClass('expanded');
				$('.menu ul:first', this).parent().find('> span a').removeClass('active');
				$('.menu ul:first', this).hide();
			});
		} else {
			$('.categories').removeClass('menu');
		}
	}).resize();
	
	// Mobile Mmenu
	var $menu = $("#menu").mmenu({
		"extensions": ["pagedim-black"],
		counters: true,
		keyboardNavigation: {
			enable: true,
			enhance: true
		},
		navbar: {
			title: 'MENU'
		},
		offCanvas: {
		  pageSelector: "#page"
	   },
		navbars: [{position:'bottom',content: ['<a href="#0">© 2022 Allaia</a>']}]}, 
		{
		// configuration
		clone: true,
		classNames: {
			fixedElements: {
				fixed: "menu_fixed"
			}
		}
	});
	
	// Menu
	$('a.open_close').on("click", function () {
		$('.main-menu').toggleClass('show');
		$('.layer').toggleClass('layer-is-visible');
	});
	$('a.show-submenu').on("click", function () {
		$(this).next().toggleClass("show_normal");
	});
	$('a.show-submenu-mega').on("click", function () {
		$(this).next().toggleClass("show_mega");
	});
	
	$('a.btn_search_mob').on("click", function () {
		$('.search_mob_wp').slideToggle("fast");
	});

	// Carousel product page
	$('.prod_pics').owlCarousel({
		items: 1,
		loop: false,
		margin: 0,
		dots:true,
		lazyLoad:true,
		nav:false
	});

	// Carousel
	$('.products_carousel').owlCarousel({
		center: false,
		items: 2,
		loop: false,
		margin: 10,
		dots:false,
		nav: true,
		lazyLoad: true,
        navText: ["<i class='ti-angle-left'></i>","<i class='ti-angle-right'></i>"],
		responsive: {
			0: {
				nav: false,
				dots:true,
				items: 2
			},
			560: {
				nav: false,
				dots:true,
				items: 3
			},
			768: {
				nav: false,
				dots:true,
				items: 4
			},
			1024: {
				items: 4
			},
			1200: {
				items: 4
			}
		}
	});
	
	// Carousels
	$('.carousel_centered').owlCarousel({
		center: true,
		items: 2,
		loop: true,
		margin: 10,
		responsive: {
			0: {
				items: 1,
				dots:false
			},
			600: {
				items: 2
			},
			1000: {
				items: 4
			}
		}
	});

	// Carousel brands
	$('#brands').owlCarousel({
		autoplay:true,
		items: 2,
		loop: true,
		margin: 10,
		dots:false,
		nav:false,
		lazyLoad: true,
		autoplayTimeout: 3000,
		responsive: {
			0: {
				items: 3
			},
			767: {
				items: 4
			},
			1000: {
				items: 6
			},
			1300: {
				items: 8
			}
		}
	});

	// Countdown offers
	$('[data-countdown]').each(function() {
	  var $this = $(this), finalDate = $(this).data('countdown');
	  $this.countdown(finalDate, function(event) {
		$this.html(event.strftime('%DD %H:%M:%S'));
	  });
	});

	// Lazy load
	var lazyLoadInstance = new LazyLoad({
	    elements_selector: ".lazy"
	});

	// Jquery select
	$('.custom-select-form select').niceSelect();

    // Product page color select
	$(".color").on('click', function () {
		$(".color").removeClass("active");
		$(this).addClass("active");
	});

	/* Input incrementer*/
	$(".numbers-row").append('<div class="inc button_inc">+</div><div class="dec button_inc">-</div>');

	$(document).on('click', '.button_inc', function () {
		var $button = $(this);
		var oldValue = $button.parent().find("input").val();
		if ($button.text() == "+") {
			var newVal = parseFloat(oldValue) + 1;
		} else {
			// Don't allow decrementing below zero
			if (oldValue > 1) {
				var newVal = parseFloat(oldValue) - 1;
			} else {
				newVal = 1;
			}
		}
		$button.parent().find("input").val(newVal);
	});
	
	/* Cart dropdown */
	$('.dropdown-cart, .dropdown-access').hover(function () {
		$(this).find('.dropdown-menu').stop(true, true).delay(50).fadeIn(300);
	}, function () {
		$(this).find('.dropdown-menu').stop(true, true).delay(50).fadeOut(300);
	});

	/* Cart Dropdown Hidden From tablet */
	$(window).bind('load resize', function () {
		var width = $(window).width();
		if (width <= 768) {
			$('a.cart_bt, a.access_link').removeAttr("data-toggle", "dropdown")
		} else {
			$('a.cart_bt,a.access_link').attr("data-toggle", "dropdown")
		}
	});

	// Opacity mask
	$('.opacity-mask').each(function(){
		$(this).css('background-color', $(this).attr('data-opacity-mask'));
	});
	
	/* Animation on scroll */
	new WOW().init();
	
	// Forgot Password
	$("#forgot").on("click", function () {
		$("#forgot_pw").fadeToggle("fast");
	});
	
	// Top panel on click: add to cart, search header
	var $topPnlSearch = $('.top_panel_search');
	var $topPnlCart = $('.top_panel_cart');
	var $pnlMsk = $('.layer');

	$('a.search_panel').on('click', function(){
		$topPnlSearch.addClass('show');
		$pnlMsk.addClass('layer-is-visible');
	});
	$('a.btn_close_top_panel_search').on('click', function(){
		$topPnlSearch.removeClass('show');
		$pnlMsk.removeClass('layer-is-visible');
	});

	$('a.btn_close_top_panel_cart').on('click', function () {
		$topPnlCart.removeClass('show');
		$pnlMsk.removeClass('layer-is-visible');
	});
	
	//Footer collapse
	var $headingFooter = $('footer h3');
	$(window).resize(function() {
        if($(window).width() <= 768) {
      		$headingFooter.attr("data-bs-toggle","collapse");
        } else {
          $headingFooter.removeAttr("data-bs-toggle","collapse");
        }
    }).resize();
	$headingFooter.on("click", function () {
		$(this).toggleClass('opened');
	});
	
	/* Footer reveal */
	//if ($(window).width() >= 1024) {
	//	$('footer.revealed').footerReveal({
	//	shadow: false,
	//	opacity:0.6,
	//	zIndex: 1
	//});
	//};

	// Scroll to top
	var pxShow = 800; // height on which the button will show
	var scrollSpeed = 500; // how slow / fast you want the button to scroll to top.
	$(window).scroll(function(){
	 if($(window).scrollTop() >= pxShow){
		$("#toTop").addClass('visible');
	 } else {
		$("#toTop").removeClass('visible');
	 }
	});
	$('#toTop').on('click', function(){
	 $('html, body').animate({scrollTop:0}, scrollSpeed);
	 return false;
	});

	// Tooltip
	 var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
		var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
		return new bootstrap.Tooltip(tooltipTriggerEl)
	})
    
    // Modal Sign In
	$('#sign-in').magnificPopup({
		type: 'inline',
		fixedContentPos: true,
		fixedBgPos: true,
		overflowY: 'auto',
		closeBtnInside: true,
		preloader: false,
		midClick: true,
		removalDelay: 300,
		closeMarkup: '<button title="%title%" type="button" class="mfp-close"></button>',
		mainClass: 'my-mfp-zoom-in'
	});
	
	// Image popups
	$('.magnific-gallery').each(function () {
		$(this).magnificPopup({
			delegate: 'a',
			type: 'image',
            preloader: true,
			gallery: {
				enabled: true
			},
			removalDelay: 500, //delay removal by X to allow out-animation
			callbacks: {
				beforeOpen: function () {
					// just a hack that adds mfp-anim class to markup 
					this.st.image.markup = this.st.image.markup.replace('mfp-figure', 'mfp-figure mfp-with-anim');
					this.st.mainClass = this.st.el.attr('data-effect');
				}
			},
			closeOnContentClick: true,
			midClick: true // allow opening popup on middle mouse click. Always set it to true if you don't provide alternative source.
		});
	});

	// Popup up
    setTimeout(function () {
        $('.popup_wrapper').css({
            "opacity": "1",
            "visibility": "visible"
        });
        $('.popup_close').on("click", function () {
            $(".popup_wrapper").fadeOut(300);
        })
	}, 1500);
})(window.jQuery); 

let cartData = [];

const getCartStorage = () => {
	return localStorage.getItem('cart') ? JSON.parse(localStorage.getItem('cart')) : [];
}

const addCart = (product) => {
	let cart = getCartStorage();
	const { variantSizeId, quantity } = product;
	let index = cart.findIndex(item => item.variantSizeId == variantSizeId);
	if (index == -1) {
		cart.push(product);
	} else {
		cart[index].quantity += quantity;
	}
	localStorage.setItem('cart', JSON.stringify(cart));
}

const removeCart = (variantSizeId) => {
	let carts = getCartStorage();
	let indexLocal = carts.findIndex(item => item.variantSizeId == variantSizeId);
	let indexData = cartData.findIndex(item => item.variantSizeId == variantSizeId);
	carts.splice(indexLocal, 1);
	cartData.splice(indexData, 1);
	localStorage.setItem('cart', JSON.stringify(carts));
	renderCart();
}

const getTotalPrice = () => {
	let totalPrice = cartData.reduce(function (sum, currentItem) {
		return sum + ((currentItem.priceSale != 0 ? currentItem.priceSale : currentItem.price)  * currentItem.quantity);
	}, 0);

	return totalPrice;
}

const getTotalProduct = () => { 
	let totalProduct = getCartStorage().reduce(function (sum, currentItem) {
		return sum + currentItem.quantity;
	}, 0);

	return totalProduct;
}

const loadCart = () => {
	const arrVariantSizeId = getCartStorage().map(item => item.variantSizeId);
	$.ajax({
		url: '/Product/GetCart',
		type: 'POST',
		contentType: 'application/json',
		data: JSON.stringify({ carts: arrVariantSizeId }),
		success: function (response) {
			cartData = mergeCartData(response);
			renderCart();
		},
		error: function (error) {
			console.error('Error:', error);
		}
	});
}

const renderCart = () => {
	let html = '';
	if (cartData.length > 0) {
		cartData.forEach(item => {
			html += `<li>
					<a href="/products/${item.productSlug}">
						<figure><img src="/img/products/${item.thumbnail}" alt="" width="80" height="80"></figure>
						<strong>
							<span>${item.quantity} x ${item.title}</span>
							<span class="text-secondary">${item.sizeName} / ${item.colorName}</span>
							<span class="d-flex gap-2 fw-bold">
								$${item.priceSale != 0 ? item.priceSale : item.price} 
								${item.priceSale != 0 ? `<span class="text-decoration-line-through">$${item.price}</span>` : ''}
							</span>
						</strong>
					</a>
					<a href="#0" class="action delete-item-cart" data-id="${item.variantSizeId}"><i class="ti-trash"></i></a></li>`
		})
	} else {
		html = '<div class="d-flex justify-content-center align-items-center gap-3 flex-column"><i class="ti ti-shopping-cart" style="font-size: 50px"></i><p>There are currently no products</p></div>'
	}
	$('.dropdown-cart .dropdown-menu ul').html(html);
	$('#cart-total-price').text(`$${getTotalPrice()}`);
	$('.cart_bt strong').text(getTotalProduct());
}

const handleChangeQuantity = (variantSizeId, quantity) => {
	let carts = getCartStorage();
	let p = carts.find(item => item.variantSizeId == variantSizeId);
	let c = cartData.find(item => item.variantSizeId == variantSizeId);
	p.quantity = quantity;
	c.quantity = quantity;
	localStorage.setItem('cart', JSON.stringify(carts));
	renderCart();
}

$(document).on('click', '.delete-item-cart', function () {
	const variantSizeId = $(this).data('id');
	removeCart(variantSizeId);
});

const mergeCartData = (productItem) => {
	const result = productItem.map((item) => {
		const quantity = getCartStorage().find(cartItem => cartItem.variantSizeId == item.variantSizeId).quantity;
		return {
			...item,
			quantity
		}
	})
	return result;
}

loadCart();