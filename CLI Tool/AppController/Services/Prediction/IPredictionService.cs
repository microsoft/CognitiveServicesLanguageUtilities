using System;
using System.Collections.Generic;
using System.Text;

namespace CustomTextCliUtils.AppController.Services.Prediction
{
    interface IPredictionService
    {
        public string Predict(string query);
    }
}
