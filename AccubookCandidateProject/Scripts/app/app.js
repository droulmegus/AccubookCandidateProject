class App {
   bookingsApiUrl = '/api/bookings';
   bookingApiUrl = '/api/booking';

   constructor() {
      this.newBookingErrorMessage = ko.observable("");
      this.bookings = ko.observableArray([]);
      this.newBooking = new Booking(0, '', new Date(), new Date(), 0, 0, '');
      this.reviews = ko.observableArray([
         new Review('Henry K.', 'I enjoyed my stay, the view was pleasant.', 4),
         new Review('John M.', 'The location was beautiful!', 3),
         new Review('Mary D.', 'Breakfast was cold. Very dissapointed.', 1),
      ]);
   }
   init() {
      ko.applyBindings(this, document.documentElement);
   }

   getBookings() {
      fetch(this.bookingsApiUrl).then(
         async (result) => {
            var bookings = (await (result && result.json()));
            if (bookings) {
               //Mapping bookings data to booking classes.
               this.bookings(bookings.map(b => new Booking(b)));
            }
         },
         () => {
            //Error handling;
         }
      );
   }

   saveBooking(newBooking) {

      //Client side check of the booking to avoid contacting the server for an invalid booking. (server side will also be done as it's API based)
      if (!newBooking.name) {
         this.newBookingErrorMessage("Provide a name");
      }
      else if (!newBooking.hotelId) {
         this.newBookingErrorMessage("Select an hotel");
      }
      else if (!newBooking.rate) {
         this.newBookingErrorMessage("Provide a hotel");
      }
      else if (!newBooking.arrival) {
         this.newBookingErrorMessage("Provide an arrivale date");
      }
      else if (!newBooking.departure) {
         this.newBookingErrorMessage("Provide a departure date");
      }
      else {

         fetch(this.bookingApiUrl, {
            body: JSON.stringify(newBooking),
            headers: {
               'Content-Type': 'application/json'
            },
            method: "POST",
         }).then((result) => {
            if (result.ok) {
               //Clearing and closing the modal
               this.newBookingErrorMessage("");
               bootstrap.Modal.getOrCreateInstance(document.querySelector("#addBooking")).hide();
               //Getting all bookings again as someoen else could have added one at the same time.
               //I would need a cache / real time notification system to lower perf issues
               this.getBookings();
            }
            else {
               this.newBookingErrorMessage("An error occured adding this booking.");
            }
         },
            () => {
               this.newBookingErrorMessage("An error occured adding this booking.");
            });
      }
   }

   averageRating() {
      var reviews = this.reviews()
      //Security check on reviews
      if (reviews && reviews.length > 0) {
         var totalRating = 0;
         //getting total reviews
         reviews.forEach(review => {
            totalRating += review.rating;
         });
         //Rounding to the two decimals. Non critical data so toFixed is fine to use.
         return (totalRating / reviews.length).toFixed(2);
      }
      return 0;
   }


   addReview() {
      var nameInput = document.querySelector("#reviewName");
      var messageInput = document.querySelector("#reviewMessage");
      var ratingInput = document.querySelector("#reviewRating");
      var errorMessageContainer = document.querySelector("#addReviewModal .text-danger");

      var newReview = new Review();
      newReview.name = nameInput.value;
      newReview.message = messageInput.value;
      newReview.rating = parseFloat(ratingInput.value);

      //Checking if review is valid
      if (!newReview.name) {
         errorMessageContainer.removeAttribute("style");
         errorMessageContainer.innerHTML = "Provide a reviewer name.";
      }
      else if (isNaN(newReview.rating)) {
         errorMessageContainer.removeAttribute("style");
         errorMessageContainer.innerHTML = "Provide a valid rating";
      }
      else {
         //Resertting modal inputs
         messageInput.value = "";
         nameInput.value = "";
         ratingInput.value = "";
         //Removing error message if one was present
         errorMessageContainer.innerHTML = "";
         errorMessageContainer.setAttribute("style", "display:none;");
         //hiding modal as the toggle was removed
         bootstrap.Modal.getOrCreateInstance(document.querySelector("#addReviewModal")).hide();
         this.reviews.push(newReview);
      }
   }
}

class Booking {
   constructor({ id, name, arrival, departure, rate, hotelId, hotelName }) {
      this.id = id;
      this.name = name;
      this.rate = rate;
      this.hotelId = hotelId;
      this.hotelName = hotelName;
      //Parsing dates to get a cleaner text shown in the table
      this.arrival = new Date(arrival);
      this.departure = new Date(departure);
   }

   /**
    * Returns numbers of days the booking is for.
    * @return {number} Number of days, rounded up.*/
   getStayDuration() {
      var stayInMs = this.departure.getTime() - this.arrival.getTime();

      //8.64e+7 millisecons in a day
      return Math.ceil(stayInMs / 8.64e+7)
   }

   getTotal() {
      return this.rate * this.getStayDuration();
   }
}

class Review {
   constructor(name, message, rating) {
      this.name = name;
      this.message = message;
      this.rating = rating;
   }
}