$(document).ready(function () {
	let productId = $("#product-id").val();
	let variants = [];

	$.get(`/Admin/Products/GetVariant/${productId}`, function (data) {
		variants = data;
		showVariant();
	})

	const showVariant = () => { 
		let html = '';
		variants.forEach((variant, index) => {
			html += `<div class="col-2 variant-color ${index == 0 ? 'active' : ''}" data-id="${index}">
						<img class="img-color mb-2" src="/img/products/${variant.thumbnail}" />
						<p class="text-center mb-2">${variant.colorName}</p>
					</div>`
		})
		$(".variant-container").html(html);
		renderSize(0);
		renderImage(0);
		setStock(0, $("#select-size").val())
	}

	const renderSize = (index) => {
		let html = '';
		variants[index].sizes.forEach((size, index) => {
			//${ size.stock == 0 ? 'disabled' : '' }
			html += `<option value="${index}">${size.sizeName} ${size.stock == 0 ? `<span>(hết hàng)</span>` : ``}</option>`;
		})
		$("#select-size").html(html);
		$("#select-size").niceSelect('update');
	}

	const renderImage = (index) => {
		let main = '';
		let thumb = '';

		variants[index].images.forEach((image, index) => {
			main += `<div style="background-image: url('/img/products/${image}')" class="item-box" ></div >`;
			thumb += `<div style="background-image: url('/img/products/${image}')" class="item ${index == 0 ? 'active' : ''}" ></div >`;
		})
		$('.owl-carousel.owl-theme.main').html(main);
		$('.owl-carousel.owl-theme.thumbs').html(thumb);
		$(".main").trigger('destroy.owl.carousel');
		$(".thumbs").trigger('destroy.owl.carousel');
		initCarousel();
	}

	$(".variant-container").on("click", ".variant-color", function (e) {
		$('.variant-color.active').removeClass("active");
		$(this).addClass("active");
		const index = $(this).data("id");
		renderSize(index);
		renderImage(index);
		setStock(index, $("#select-size").val())
	});

	$("#select-size").change(function (e) {
		let variantIndex = $(".variant-color.active").data("id");
		let sizeIndex = $(this).val();
		setStock(variantIndex, sizeIndex);
	});

	const setStock = (variantIndex, sizeIndex) => {
		const stock = variants[variantIndex].sizes[sizeIndex].stock;
		if (stock > 0) {
			$(".color-stock").html(`Còn hàng (${stock})`);
			$(".btn-cart-container").html(`<div class="btn_add_to_cart"><a href="#0" class="btn_1 add-cart">Add to Cart</a></div>`)
		} else {
			$(".color-stock").html(`Hết hàng`);
			$(".btn-cart-container").html(`<div class="btn_add_to_cart"><a href="#0" class="btn_1 bg-danger">Hết hàng</a></div>`)
		}
		
	}


	$(document).on("click", ".btn_add_to_cart .add-cart", function (e) {
		const size_index = $("#select-size").val();
		const color_index = $('.variant-color.active').data("id");

		const variant = variants[color_index].sizes[size_index];
		const stock = variant.stock;
		const quantity = parseInt($('#quantity_1').val());
		const cartItem = getCartStorage().find(item => item.variantSizeId == variant.variantSizeId);

		const cartQuantity = cartItem ? cartItem.quantity : 0;

		if (quantity + cartQuantity > stock) {
			if (quantity > stock) {
				Swal.fire({
					icon: "error",
					title: "Oops...",
					text: "The product does not have enough inventory"
				});
			} else {
				Swal.fire({
					icon: "error",
					title: "Oops...",
					text: "The product already exists in the shopping cart"
				});
			}
			
		} else {
			const obj = {
				title: $('h1').text(),
				thumbnail: variants[color_index].thumbnail,
				colorName: variants[color_index].colorName,
				variantSizeId: variants[color_index].sizes[size_index].variantSizeId,
				sizeName: variants[color_index].sizes[size_index].sizeName,
				price: $('.product-price').data("price"),
				quantity: quantity
			};

			const cartItem = { variantSizeId: obj.variantSizeId, quantity: obj.quantity };
			addCart(cartItem);
			loadCart();
			showCartTopBar(obj);
			var $topPnlCart = $('.top_panel_cart');
			var $pnlMsk = $('.layer');
			$topPnlCart.addClass('show');
			$pnlMsk.addClass('layer-is-visible');
		}
	});

	const showCartTopBar = (product) => {
		let html = `<figure>
								<img src="/img/products/${product.thumbnail}" alt="" data-was-processed="true">
					</figure>
					<h4>${product.quantity} x ${product.title}</h4>
					<p class="mb-0 text-secondary">Color: ${product.colorName}, Size: ${product.sizeName}</p>
					<div class="price_panel"><span class="new_price">$${product.price}</span></div>`;
		$('.pnl-cart').html(html);
	}

	const initCarousel = () => {
		var changeSlide = 4; 
		var slide = changeSlide;
		if ($(window).width() < 600) {
			var slide = changeSlide;
			slide--;
		} else if ($(window).width() > 999) {
			var slide = changeSlide;
			slide++;
		} else {
			var slide = changeSlide;
		}
		$(".main").owlCarousel({
			nav: true,
			items: 1
		});
		$(".thumbs").owlCarousel({
			nav: true,
			margin: 15,
			mouseDrag: false,
			touchDrag: true,
			responsive: {
				0: {
					items: changeSlide - 1,
					slideBy: changeSlide - 1
				},
				600: {
					items: changeSlide,
					slideBy: changeSlide
				},
				1000: {
					items: changeSlide + 1,
					slideBy: changeSlide + 1
				}
			}
		});
		var owl = $(".main");
		owl.owlCarousel();
		owl.on("translated.owl.carousel", function (event) {
			$(".right").removeClass("nonr");
			$(".left").removeClass("nonl");
			if ($(".main .owl-next").is(".disabled")) {
				$(".slider .right").addClass("nonr");
			}
			if ($(".main .owl-prev").is(".disabled")) {
				$(".slider .left").addClass("nonl");
			}
			$(".slider-two .item").removeClass("active");
			var c = $(".slider .owl-item.active").index();
			$(".slider-two .item")
				.eq(c)
				.addClass("active");
			var d = Math.ceil((c + 1) / slide) - 1;
			$(".slider-two .owl-dots .owl-dot")
				.eq(d)
				.trigger("click");
		});
		$(".right").click(function () {
			$(".slider .owl-next").trigger("click");
		});
		$(".left").click(function () {
			$(".slider .owl-prev").trigger("click");
		});
		$(".slider-two .item").click(function () {
			var b = $(".item").index(this);
			$(".slider .owl-dots .owl-dot")
				.eq(b)
				.trigger("click");
			$(".slider-two .item").removeClass("active");
			$(this).addClass("active");
		});
		var owl2 = $(".thumbs");
		owl2.owlCarousel();
		owl2.on("translated.owl.carousel", function (event) {
			$(".right-t").removeClass("nonr-t");
			$(".left-t").removeClass("nonl-t");
			if ($(".two .owl-next").is(".disabled")) {
				$(".slider-two .right-t").addClass("nonr-t");
			}
			if ($(".thumbs .owl-prev").is(".disabled")) {
				$(".slider-two .left-t").addClass("nonl-t");
			}
		});
		$(".right-t").click(function () {
			$(".slider-two .owl-next").trigger("click");
		});
		$(".left-t").click(function () {
			$(".slider-two .owl-prev").trigger("click");
		});
	}
});