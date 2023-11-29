
!function () {
    class t {
        static initOverviewChart(data) {
            Chart.defaults.color = "#818d96";
            Chart.defaults.scale.grid.color = "transparent";
            Chart.defaults.scale.grid.zeroLineColor = "transparent";
            Chart.defaults.scale.beginAtZero = !0;
            Chart.defaults.elements.point.radius = 0;
            Chart.defaults.elements.point.hoverRadius = 0;
            Chart.defaults.plugins.tooltip.radius = 3;
            Chart.defaults.plugins.legend.labels.boxWidth = 12;

            let o = document.getElementById("js-chartjs-overview");
            if (null !== o) {
                let r = {
                    responsive: !0,
                    maintainAspectRatio: !1,
                    tension: .4,
                    scales: {
                        y: {
                            suggestedMin: 0,
                            suggestedMax: 600
                        }
                    },
                    interaction: {
                        intersect: !1
                    },
                    plugins: {
                        tooltip: {
                            callbacks: {
                                label: function (t) {
                                    return "$ " + t.parsed.y
                                }
                            }
                        }
                    }
                };

                let e = {
                    labels: ["MON", "TUE", "WED", "THU", "FRI", "SAT", "SUN"],
                    datasets: [{
                        label: "This Week",
                        fill: !0,
                        backgroundColor: "rgba(6, 101, 208, .5)",
                        borderColor: "transparent",
                        pointBackgroundColor: "rgba(6, 101, 208, 1)",
                        pointBorderColor: "#fff",
                        pointHoverBackgroundColor: "#fff",
                        pointHoverBorderColor: "rgba(6, 101, 208, 1)",
                        data: data.currentWeek
                    }, {
                        label: "Last Week",
                        fill: !0,
                        backgroundColor: "rgba(6, 101, 208, .2)",
                        borderColor: "transparent",
                        pointBackgroundColor: "rgba(6, 101, 208, .2)",
                        pointBorderColor: "#fff",
                        pointHoverBackgroundColor: "#fff",
                        pointHoverBorderColor: "rgba(6, 101, 208, .2)",
                        data: data.previousWeek
                    }]
                };

                let n = new Chart(o, {
                    type: "line",
                    data: e,
                    options: r
                });
            }
        }

        static fetchDataAndInitOverviewChart() {
            $.ajax({
                url: '/Admin/Home/GetStatistic',
                type: 'GET',
                dataType: 'json',
                success: function (data) {
                    t.initOverviewChart(data);
                },
                error: function (error) {
                    console.error('Error fetching data:', error);
                }
            });
        }

        static init() {
            this.fetchDataAndInitOverviewChart();
        }
    }

    Dashmix.onLoad((() => t.init()))
}();