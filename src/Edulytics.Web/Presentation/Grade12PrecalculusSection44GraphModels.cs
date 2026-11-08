using Edulytics.Core.Mathematics.Visuals;
namespace Edulytics.Web.Presentation;
/// <summary>Original first-party graph skills, not publisher figures; runtime to be checked in Stage D.</summary>
public static class Grade12PrecalculusSection44GraphModels
{
    public sealed record GraphTask(int SourceFigureSkill, string Prompt, CoordinatePlaneVisualSpec Graph);
    public static readonly IReadOnlyList<GraphTask> All=Build();
    private static IReadOnlyList<GraphTask> Build()
    {
        var items=new List<GraphTask>();
        Add(1, "Growth parent: independent logarithmic graph",
            [x => x>0m ? (decimal?)((decimal)(Math.Log((double)x,2d))) : null]);
        Add(2, "Decay parent: independent logarithmic graph",
            [x => x>0m ? (decimal?)((decimal)(Math.Log((double)x,0.5d))) : null]);
        Add(3, "Left shift: independent logarithmic graph",
            [x => x>-1m ? (decimal?)((decimal)(Math.Log((double)(x+1m),2d))) : null]);
        Add(4, "Reflected input: independent logarithmic graph",
            [x => x<3m ? (decimal?)((decimal)(Math.Log((double)(3m-x)))) : null]);
        Add(5, "Right shift: independent logarithmic graph",
            [x => x>2m ? (decimal?)((decimal)(Math.Log((double)(x-2m),3d))) : null]);
        Add(6, "Vertical shift: independent logarithmic graph",
            [x => x>0m ? (decimal?)((decimal)(Math.Log((double)x)+2d)) : null]);
        Add(7, "Shift down: independent logarithmic graph",
            [x => x>1m ? (decimal?)((decimal)(Math.Log((double)(x-1m),4d)-2d)) : null]);
        Add(8, "Stretch: independent logarithmic graph",
            [x => x>0m ? (decimal?)((decimal)(2d*Math.Log((double)x,2d))) : null]);
        Add(9, "Reflect x-axis: independent logarithmic graph",
            [x => x>0m ? (decimal?)((decimal)(-Math.Log((double)x,2d))) : null]);
        Add(10, "Reflect y-axis: independent logarithmic graph",
            [x => x<0m ? (decimal?)((decimal)(Math.Log((double)(-x),2d))) : null]);
        Add(11, "Translate left: independent logarithmic graph",
            [x => x>-3m ? (decimal?)((decimal)(Math.Log((double)(x+3m)))) : null]);
        Add(12, "Translate right: independent logarithmic graph",
            [x => x>4m ? (decimal?)((decimal)(Math.Log((double)(x-4m),5d))) : null]);
        Add(13, "Double reflection: independent logarithmic graph",
            [x => x<0m ? (decimal?)((decimal)(-Math.Log((double)(-x),3d))) : null]);
        Add(14, "Translate up: independent logarithmic graph",
            [x => x>0m ? (decimal?)((decimal)(Math.Log((double)x,4d)+1d)) : null]);
        Add(15, "Input reversal with shift: independent logarithmic graph",
            [x => x<2m ? (decimal?)((decimal)(Math.Log((double)(2m-x)))) : null]);
        Add(16, "Asymptote shift: independent logarithmic graph",
            [x => x>1m ? (decimal?)((decimal)(Math.Log((double)(x-1m),2d))) : null]);
        Add(17, "Compare multiple independently authored logarithmic curves",
            [x => x>0m ? (decimal?)((decimal)(Math.Log((double)x,2d))) : null, x => x>0m ? (decimal?)((decimal)(Math.Log((double)x))) : null, x => x>0m ? (decimal?)((decimal)(Math.Log((double)x,5d))) : null]);
        Add(18, "Compare multiple independently authored logarithmic curves",
            [x => x>0m ? (decimal?)((decimal)(Math.Log((double)x,1d/3d))) : null, x => x>0m ? (decimal?)((decimal)(Math.Log((double)x,2d))) : null, x => x>0m ? (decimal?)((decimal)(Math.Log((double)x,0.75d))) : null]);
        Add(19, "Compare multiple independently authored logarithmic curves",
            [x => x<2m ? (decimal?)((decimal)(Math.Log((double)(2m-x),3d))) : null, x => x>-2m ? (decimal?)((decimal)(-Math.Log((double)(x+2m),3d))) : null, x => x>-2m ? (decimal?)((decimal)(Math.Log((double)(x+2m),3d))) : null]);
        return items;
        void Add(int number, string prompt, Func<decimal,decimal?>[] funcs)
        {
            var segments=new List<MathematicalSegment>();
            foreach(var f in funcs)
            {
                MathematicalPoint? previous=null;
                for(var i=0;i<=120/Math.Max(1,funcs.Length);i++)
                {
                    var x=-7m+21m*i/(120m/Math.Max(1,funcs.Length));
                    var value=f(x);
                    if(!value.HasValue || value.Value<-9m || value.Value>9m)
                    { previous=null; continue; }
                    var point=new MathematicalPoint(x,value.Value);
                    if(previous is not null) segments.Add(new MathematicalSegment(previous,point));
                    previous=point;
                }
            }
            var graph=new CoordinatePlaneVisualSpec(
                new MathematicalAxisWindow(-7m,14m,-9m,9m,2m,2m),
                [],segments,new MathematicalVisualAccessibility(
                    $"First-party logarithm figure skill {number}",
                    prompt+". Coordinate plane plot authored for Edulytics, not copied from the publisher."));
            MathematicalVisualSpecValidator.Validate(graph);
            items.Add(new GraphTask(number,prompt,graph));
        }
    }
}
