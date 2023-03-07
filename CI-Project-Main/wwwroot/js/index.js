let menu = document.querySelector("#menu");
let sidebar = document.querySelector(".sidebar-filter");
let close = document.querySelector("#close-btn");
var checkboxes = document.querySelectorAll(".checkbox");
let filtersSection = document.querySelector(".filters-section");
// span
var filterList = document.querySelector(".filter-list");
// Get the elements with class="column"
var elements = document.getElementsByClassName("column1");
var cards = document.getElementsByClassName("card");
let commentText = document.getElementsByTagName("textarea").value;

let parentCommentDiv = document.querySelector('.comment-list');
let postButton = document.querySelector('.post-comment');
let navbarSearchIconInSmallDevices = document.querySelector('#navbar-search-in-small-devices');
let searchInputNavbarSmall = document.querySelector('#search-input-navbar-small');



for (var checkbox of checkboxes) {
    checkbox.addEventListener("click", function () {
        if (this.checked == true) {

            addElement(this, this.value);

        }
        else {

            removeElement(this.value);

        }
    })
}

//this function will add filters

function addElement(current, value) {
    let filtersSection = document.querySelector(".filters-section");

    let createdTag = document.createElement('span');
    createdTag.classList.add('filter-list');
    createdTag.classList.add('ps-3');
    createdTag.classList.add('pe-1');
    createdTag.classList.add('me-2');
    createdTag.innerHTML = value;
   
    createdTag.setAttribute('id', value);
    let crossButton = document.createElement('button');
    crossButton.classList.add("filter-close-button");
    let cross = '&times;'

    crossButton.addEventListener('click', function () {
        let elementToBeRemoved = document.getElementById(value);

        console.log(elementToBeRemoved);
        console.log(current);
        elementToBeRemoved.remove();

        current.checked = false;
    })

    crossButton.innerHTML = cross;

    createdTag.appendChild(crossButton);
    filtersSection.appendChild(createdTag);

}

function removeElement(value) {

    let filtersSection = document.querySelector(".filters-section");

    let elementToBeRemoved = document.getElementById(value);
    filtersSection.removeChild(elementToBeRemoved);

}

// Declare a loop variable
var i;


navbarSearchIconInSmallDevices.addEventListener("click", () => {

    searchInputNavbarSmall.classList.toggle('open-search');
})




//add comments
let commentToBeAdded = `<div class="single-comment d-flex gap-3">
<div class="user-profile-image">
    <img src="images/Assets/volunteer1.png" alt="">
</div>
<div class="user-details">
    <p id="comment-user-name" class="mt-1">User name</p>
    <p id="comment-posted-date" class="mt-1">Date posted</p>
    <p id="comment-actual-text" class="mt-3">${commentText}</p>
</div>
</div>`

var text;
function getVal(event) {
    text = event.target.value;

}

//When you add mission detail page : uncomment this 

//postButton.addEventListener('click', () => {
//    //text : is user comment
//    let parentCommentDiv = document.querySelector('.comment-list');

//    let mainDiv = document.createElement('div');
//    mainDiv.classList.add('single-comment');
//    mainDiv.classList.add('d-flex');
//    mainDiv.classList.add('gap-3');

//    parentCommentDiv.appendChild(mainDiv);

//    let childDiv = document.createElement('div');
//    childDiv.classList.add('user-profile-image');

//    let imageTag = document.createElement('img');
//    imageTag.setAttribute('src', 'images/Assets/volunteer1.png');


//    childDiv.appendChild(imageTag);

//    mainDiv.appendChild(childDiv);

//    let commentLists = document.createElement('div');
//    commentLists.classList.add('user-details');


//    let detailsOfUserComment_name = document.createElement('p');
//    detailsOfUserComment_name.classList.add('mt-1');
//    detailsOfUserComment_name.setAttribute('id', 'comment-user-name');
//    detailsOfUserComment_name.innerHTML = "Kaushal oza";

//    commentLists.appendChild(detailsOfUserComment_name);

//    let detailsOfUserComment_date = document.createElement('p');
//    detailsOfUserComment_date.classList.add('mt-1');
//    detailsOfUserComment_date.setAttribute('id', 'comment-posted-date');
//    detailsOfUserComment_date.innerHTML = "17/05/2002";
//    commentLists.appendChild(detailsOfUserComment_date);



//    let detailsOfUserComment_text = document.createElement('p');
//    detailsOfUserComment_text.classList.add('mt-3');
//    detailsOfUserComment_text.setAttribute('id', 'comment-actual-text');
//    detailsOfUserComment_text.innerHTML = text;

//    commentLists.appendChild(detailsOfUserComment_text);


//    mainDiv.appendChild(commentLists);

//})

menu.addEventListener("click", () => {
    sidebar.classList.toggle("show-sidebar");
})

close.addEventListener("click", () => {
    sidebar.classList.remove("show-sidebar");
})


//list view - grid view
$(document).ready(function () {
    $('#list').click(function (event) { event.preventDefault(); $('#products .item').addClass('list-group-item'); });
    $('#grid').click(function (event) { event.preventDefault(); $('#products .item').removeClass('list-group-item'); $('#products .item').addClass('grid-group-item'); });
});

