$(document).ready(function () {
	let productId = $("#product-id").val();
	let variants = [];

	$.get(`/Admin/Products/GetVariant/${productId}`, function (data) {
		variants = data;
		console.log(variants)
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
	}

	const renderSize = (index) => {
		let html = '';
		variants[index].sizes.forEach(size => {
			html += `<option value="${size.sizeId}">${size.sizeName}</option>`;
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
	});

	const initCarousel = () => {

		var changeSlide = 4; // mobile -1, desktop + 1
		// Resize and refresh page. slider-two slideBy bug remove
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