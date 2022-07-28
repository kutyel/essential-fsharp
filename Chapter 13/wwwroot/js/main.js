// Create a "close" button and append it to each list item
const myNodelist = document.getElementsByTagName("LI");

Array.from(myNodelist).forEach((node) => {
  const span = document.createElement("SPAN");
  span.className = "close";
  span.appendChild(document.createTextNode("\u00D7"));
  node.appendChild(span);
});

// Click on a close button to hide the current list item
const close = document.getElementsByClassName("close");

Array.from(close).forEach(
  (c) =>
    (c.onclick = function () {
      this.parentElement.style.display = "none";
    })
);

// Add a "checked" symbol when clicking on a list item
const list = document.querySelector("ul");

list.addEventListener(
  "click",
  (ev) => {
    if (ev.target.tagName === "LI") {
      ev.target.classList.toggle("checked");
    }
  },
  false
);

// Create a new list item when clicking on the "Add" button
function newElement() {
  const li = document.createElement("li");
  const inputValue = document.getElementById("myInput").value;
  const t = document.createTextNode(inputValue);
  li.appendChild(t);

  if (inputValue === "") {
    alert("You must write something!");
  } else {
    document.getElementById("myUL").appendChild(li);
  }

  document.getElementById("myInput").value = "";

  const span = document.createElement("SPAN");
  const txt = document.createTextNode("\u00D7");
  span.className = "close";
  span.appendChild(txt);
  li.appendChild(span);

  Array.from(close).forEach(
    (c) =>
      (c.onclick = function () {
        this.parentElement.style.display = "none";
      })
  );
}
