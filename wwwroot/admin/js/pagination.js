class Pagination {
    constructor(container, link, fetchData, inputSearch) {
        this.container =
            container || document.querySelector(".pagination-container");
        (this.fetchData = fetchData), (this.link = link);
        this.inputSearch = inputSearch;
        this.loadContainer();
        this.pg = this.container.querySelector(".list-page");
        this.btnPrevPg = this.container.querySelector("a.prev-page");
        this.btnNextPg = this.container.querySelector("a.next-page");
        this.btnFirstPg = this.container.querySelector("a.first-page");
        this.btnLastPg = this.container.querySelector("a.last-page");
        this.inputSearch = document.getElementById(inputSearch) || null;

        this.valuePage = {
            currentPage: 1,
            query: "",
            totalPages: 0,
            result: null,
        };
        this.getPagination();
        this.container.addEventListener("click", this.containerHandler.bind(this));

        if (this.inputSearch) {
            // Áp dụng debounce cho sự kiện change
            const debouncedChangeHandler = this.debounce((e) => {
                this.valuePage.currentPage = 1;
                this.valuePage.query = e.target.value;
                this.updateURL();
                this.getPagination();
            }, 500);
            this.inputSearch.addEventListener("input", function (event) {
                debouncedChangeHandler(event);
            });
        }

        window.addEventListener("popstate", (event) => {
            const state = event.state;
            if (state) {
                this.valuePage.currentPage = state.currentPage;
                this.valuePage.query = state.query;
                this.inputSearch.value = this.valuePage.query
                this.getPagination();
            }
        });

    }

    updateURL() {
        const stateObj = {
            currentPage: this.valuePage.currentPage,
            query: this.valuePage.query
        };
        if (this.valuePage.query != 0) {
            const newURL = `${window.location.pathname}?page=${this.valuePage.currentPage}&query=${this.valuePage.query}`;
            history.pushState(stateObj, null, newURL);
        } else {
            const stateOb2 = {
                currentPage: this.valuePage.currentPage,
            };
            const newURL2 = `${window.location.pathname}?page=${this.valuePage.currentPage}`;
            history.pushState(stateOb2, null, newURL2);
        }
    }


    useFetchData(data) {
        this.fetchData(data);
    }

    loadContainer = () => {
        this.container.innerHTML = `<ul class="pagination justify-content-end mt-2">
                        <li class="page-item">
                            <a class="page-link first-page disabled" href="javascript:void(0)" tabindex="-1" aria-label="First" data-page="1">
                                <i class="fas fa-angle-double-left"></i>
                            </a>
                        </li>
                        <li class="page-item">
                            <a class="page-link prev-page disabled" href="javascript:void(0)" tabindex="-1" aria-label="Previous">
                                <i class="fas fa-angle-left"></i>
                            </a>
                        </li>
                        <div class="d-flex list-page">
                            <li class="page-item active">
                                <a class="page-link" href="javascript:void(0)" style="border-radius:0;" data-page="1">1</a>
                            </li>
                        </div>
                        <li class="page-item">
                            <a class="page-link next-page disabled" href="javascript:void(0)" tabindex="-1" aria-label="Next">
                                <i class="fas fa-angle-right"></i>
                            </a>
                        </li>
                        <li class="page-item">
                            <a class="page-link last-page disabled" href="javascript:void(0)" tabindex="-1" aria-label="Last">
                                <i class="fas fa-angle-double-right"></i>
                            </a>
                        </li>
                    </ul>`;
    };

    getPagination() {
        const self = this;
        // Parse currentPage và query từ URL thay vì sử dụng self.valuePage
        const params = new URLSearchParams(window.location.search);
        self.valuePage.currentPage = parseInt(params.get("page")) || 1;
        self.valuePage.query = params.get("query") || "";
        

        let linkSend =
            this.link +
            "?page=" +
            self.valuePage.currentPage +
            "&query=" +
            self.valuePage.query;

        $.ajax({
            url: linkSend,
            method: "get",
            dataType: "json",
            success: function (response) {
                console.log('response', response)
                self.valuePage.currentPage = response.currentPage;
                self.valuePage.totalPages = response.totalPages;
                self.valuePage.result = response.result;
                self.useFetchData(self.valuePage.result);
                self.pagination();
            },
            error: function (err) {
                console.error(err.responseText);
            },
        });
    }

    debounce(func, delay) {
        let timeoutId;
        return function (...args) {
            const context = this;
            clearTimeout(timeoutId);
            timeoutId = setTimeout(() => {
                func.apply(context, args);
            }, delay);
        };
    }

    containerHandler(e) {
        if (e.target.closest(".page-link")) {
            this.handleButton(e.target.closest(".page-link"));
            this.getPagination();
        }
    }

    renderPage(index, active = "") {
        let style = "";
        if (index === 1 || index === this.valuePage.totalPages) {
            style = `style="border-radius:0;"`;
        }
        return `<li class="page-item ${active}">
          <a class="page-link" href="javascript:void(0)" ${style ? style : ""
            } data-page="${index}">${index}</a>
      </li>`;
    }

    pagination() {
        const {
            totalPages,
            currentPage
        } = this.valuePage;
        let render = "";
        for (let pos = 1; pos <= totalPages; pos++) {
            if (pos == currentPage) render += this.renderPage(pos, "active");
            else render += this.renderPage(pos, "");
        }
        this.pg.innerHTML = render;

        this.handleButtonLeft();
        this.handleButtonRight();
    }

    handleButtonLeft() {
        if (this.valuePage.currentPage === 1 || this.valuePage.totalPages <= 1) {
            this.btnPrevPg.classList.add("disabled");
            this.btnFirstPg.classList.add("disabled");
        } else {
            this.btnPrevPg.classList.remove("disabled");
            this.btnFirstPg.classList.remove("disabled");
        }
    }

    handleButtonRight() {
        if (
            this.valuePage.currentPage === this.valuePage.totalPages ||
            this.valuePage.totalPages <= 1
        ) {
            this.btnNextPg.classList.add("disabled");
            this.btnLastPg.classList.add("disabled");
        } else {
            this.btnNextPg.classList.remove("disabled");
            this.btnLastPg.classList.remove("disabled");
        }
    }

    handleButton(element) {
        if (element.classList.contains("first-page")) {
            this.valuePage.currentPage = 1;
        } else if (element.classList.contains("last-page")) {
            this.valuePage.currentPage = this.valuePage.totalPages;
        } else if (element.classList.contains("prev-page")) {
            if (this.valuePage.currentPage === 1) return;
            this.valuePage.currentPage--;
            this.btnNextPg.classList.remove("disabled");
            this.btnLastPg.classList.remove("disabled");
        } else if (element.classList.contains("next-page")) {
            if (this.valuePage.currentPage === this.valuePage.totalPages) return;
            this.valuePage.currentPage++;
            this.btnPrevPg.classList.remove("disabled");
            this.btnFirstPg.classList.remove("disabled");
        } else {
            let pageId = element.getAttribute("data-page");
            this.valuePage.currentPage = pageId;
        }
        this.pagination();
        this.handleButtonLeft();
        this.handleButtonRight();
        this.updateURL();
    }
}