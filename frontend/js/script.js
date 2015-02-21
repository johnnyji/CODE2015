$(document).ready(function() {
	setTimeout(function() {
		$(".result").addClass("animated bounceInLeft");
		setTimeout(function() {
			$(".result").removeClass("animated bounceInLeft");
		}, 1000);
	}, 500);

	$(window).scroll(function() {
		$(".fa-chevron-down").fadeOut(400);
	});
});