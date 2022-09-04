const hamburger = document.querySelector(".hamburger")

const backdrop = document.querySelector(".backdrop")

const sidebar = document.querySelector(".sidebar")

const toggleSidebar = () => {
    backdrop.classList.toggle('backdrop--active');
    sidebar.classList.toggle('sidebar--active')
}

function myFunction() {
    document.getElementById("myDropdown").classList.toggle("show");
}

// Close the dropdown menu if the user clicks outside of it
window.onclick = function (event) {
    if (!event.target.matches('.dropbtn')) {
        var dropdowns = document.getElementsByClassName("dropdown-content");
        var i;
        for (i = 0; i < dropdowns.length; i++) {
            var openDropdown = dropdowns[i];
            if (openDropdown.classList.contains('show')) {
                openDropdown.classList.remove('show');
            }
        }
    }
}


function setHeiHeight() {
    const windowInnerHeight = window.innerHeight
    const windowInnreWidht = window.innerWidth
    if(windowInnreWidht <= 404){
        let heightBlock = document.getElementById('blockWithAutoWidthOrientation').clientHeight;
        document.getElementById('blockWithAutoWidth').style.height = 100 + '%';
    }
    else{
        document.getElementById('blockWithAutoWidth').style.height = (windowInnerHeight - 80) + 'px';
    }
}
setHeiHeight(); // устанавливаем высоту окна при первой загрузке страницы
window.onresize = setHeiHeight;  // обновляем при изменении размеров окна



function OpenFilters() {
    var button = document.getElementById("filterButton");

    button.classList.toggle("active");
    var content = document.getElementById("filterContent");

    if (content.style.display === "block") {
      content.style.display = "none";
    } else {
      content.style.display = "block";
    }
}