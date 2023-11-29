$(document).ready(function () {
	$("#form-review").validate({
		rules: {
			review: {
				required: true
			}
		},
		messages: {
			review: {
				required: "Please enter your review"
			}
		},
		submitHandler: function (form) {
			var overallRating = $("input[name='rating-input']:checked").val();
			var reviewText = $("textarea[name='review']").val();
			var productId = $("#product-id").val();

			$.ajax({
				type: "POST",
				url: "/Product/AddReview",
				data: {
					ProductId: productId,
					Description: reviewText,
					Rating: overallRating
				},
				success: function (response) {
					connection.invoke("SendComment", productId).catch(err => console.error(err));
					var myModal = bootstrap.Modal.getOrCreateInstance(document.getElementById('modal-add-review'));

					myModal.hide();
				},
				error: function (error) {
					console.error("Error in POST request", error);
				}
			});
		}
	});

	const getDetailRating = (productId) => {
		$.ajax({
			type: "GET",
			url: `/Product/getDetailStar/${productId}`,
			success: function (data) {
				let ratingHtml = ` <div class="text-center">
		  <div class="fs-3">
			<strong class="text-warning">${data.averageRating}</strong>/ <span>5</span>
		  </div>
		  <h6 class="text-center">Có ${data.countView} lượt đánh giá</h6>
		</div>
		<div class="d-flex justify-content-between align-items-center">
		  <span class="rating me-3">
			<i class="icon-star text-warning"></i>
			<i class="icon-star"></i>
			<i class="icon-star"></i>
			<i class="icon-star"></i>
			<i class="icon-star"></i>
		  </span>
		  <div class="progress flex-grow-1 mb-2">
			<div class="progress-bar" role="progressbar" style="width: calc(${data.reviewStats.percentOneStar})" aria-valuenow="${data.reviewStats.oneStar}" aria-valuemin="0" aria-valuemax="${data.totalRating}"> ${data.reviewStats.percentOneStar} </div>
		  </div>
		</div>
		<div class="d-flex justify-content-between align-items-center">
		  <span class="rating me-3">
			<i class="icon-star text-warning"></i>
			<i class="icon-star text-warning"></i>
			<i class="icon-star"></i>
			<i class="icon-star"></i>
			<i class="icon-star"></i>
		  </span>
		  <div class="progress flex-grow-1 mb-2">
			<div class="progress-bar" role="progressbar" style="width: calc(${data.reviewStats.percentTwoStar})" aria-valuenow="${data.reviewStats.twoStar}" aria-valuemin="0" aria-valuemax="${data.totalRating}"> ${data.reviewStats.percentTwoStar} </div>
		  </div>
		</div>
		<div class="d-flex justify-content-between align-items-center">
		  <span class="rating me-3">
			<i class="icon-star text-warning"></i>
			<i class="icon-star text-warning"></i>
			<i class="icon-star text-warning"></i>
			<i class="icon-star"></i>
			<i class="icon-star"></i>
		  </span>
		  <div class="progress flex-grow-1 mb-2">
			<div class="progress-bar" role="progressbar" style="width: calc(${data.reviewStats.percentThreeStar})" aria-valuenow="${data.reviewStats.threeStar}" aria-valuemin="0" aria-valuemax="${data.totalRating}"> ${data.reviewStats.percentThreeStar} </div>
		  </div>
		</div>
		<div class="d-flex justify-content-between align-items-center">
		  <span class="rating me-3">
			<i class="icon-star text-warning"></i>
			<i class="icon-star text-warning"></i>
			<i class="icon-star text-warning"></i>
			<i class="icon-star text-warning"></i>
			<i class="icon-star"></i>
		  </span>
		  <div class="progress flex-grow-1 mb-2">
			<div class="progress-bar" role="progressbar" style="width: calc(${data.reviewStats.percentFourStar})" aria-valuenow="${data.reviewStats.fourStar}" aria-valuemin="0" aria-valuemax="${data.totalRating}"> ${data.reviewStats.percentFourStar} </div>
		  </div>
		</div>
		<div class="d-flex justify-content-between align-items-center">
		  <span class="rating me-3">
			<i class="icon-star text-warning"></i>
			<i class="icon-star text-warning"></i>
			<i class="icon-star text-warning"></i>
			<i class="icon-star text-warning"></i>
			<i class="icon-star"></i>
		  </span>
		  <div class="progress flex-grow-1 mb-2">
			<div class="progress-bar" role="progressbar" style="width: calc(${data.reviewStats.percentFiveStar})" aria-valuenow="${data.reviewStats.fiveStar}" aria-valuemin="0" aria-valuemax="${data.totalRating}"> ${data.reviewStats.percentFiveStar} </div>
		  </div>
		</div>`
				$("#detail_rating").html(ratingHtml);
			},
			error: function (error) {
				console.error("Error in Get request", error);
			}
		})
	};


	const connection = new signalR.HubConnectionBuilder()
		.withUrl("/commentHub")
		.build();

	connection.start().then(() => {
		var productId = $("#product-id").val();
		connection.invoke("JoinGroup", productId).catch(err => console.error(err));
	}).catch(err => console.error(err));

	connection.on("ReceiveComment", (comment) => {
		var productId = $("#product-id").val();
		getDetailRating(productId);
		const inputDateString = comment.commentCreatedAt;

		const inputDate = new Date(inputDateString);

		const formattedDate = inputDate.toLocaleString('en-US', {
			day: 'numeric',
			month: 'numeric',
			year: 'numeric',
			hour: 'numeric',
			minute: 'numeric',
			second: 'numeric',
			hour12: true
		});
		let rating = ``;

		for (let i = 1; i <= comment.commentRating; i++) {
			rating += '<i class="icon-star"></i>';
		}

		for (let i = 1; i <= (5 - comment.commentRating); i++) {
			rating += '<i class="icon-star empty"></i>';
		}

		let commentHtml = `<div class="review_content">
				<div class="clearfix add_bottom_10">
					<span class="rating">
						${rating}
						<em>${comment.commentRating}/5.0</em>
					</span>
							<em>Published ${formattedDate}</em>
				</div>
				<div class="d-flex align-items-center">
					<span style="font-size: 16px; font-weight: bold">${comment.commentName}</span>
					<img style="width: 18px; height: 18px; margin-left: 10px; margin-right: 5px" src="/img/buy_product.webp" alt="Đã mua hàng"> Đã mua hàng
				</div>
				<p>${comment.commentText}</p>
			</div>`;

		$("#list_comment").prepend(commentHtml);
	});

});