import Link from 'next/link'
import { ChefHat, Trophy, Brain, Clock } from 'lucide-react'
import { getQuizzes, getCategories } from '@/lib/cosmic'
import QuizCard from '@/components/QuizCard'
import CategoryFilter from '@/components/CategoryFilter'

export default async function HomePage() {
  const [quizzes, categories] = await Promise.all([
    getQuizzes(),
    getCategories()
  ])

  const featuredQuizzes = quizzes.slice(0, 3)

  return (
    <div className="min-h-screen">
      {/* Hero Section */}
      <section className="bg-gradient-to-r from-primary to-primary-dark text-white py-20">
        <div className="container mx-auto px-4">
          <div className="max-w-4xl mx-auto text-center">
            <ChefHat className="w-20 h-20 mx-auto mb-6" />
            <h1 className="text-5xl font-bold mb-6">
              Master Your Culinary Knowledge
            </h1>
            <p className="text-xl mb-8 opacity-95">
              Test your cooking expertise with our engaging quizzes about recipes, 
              cuisines, ingredients, and techniques. Compete with fellow food enthusiasts!
            </p>
            <div className="flex gap-4 justify-center">
              <Link
                href="/quizzes"
                className="bg-white text-primary px-8 py-4 rounded-lg font-semibold hover:bg-gray-100 transition-colors"
              >
                Start Quiz
              </Link>
              <Link
                href="/leaderboard"
                className="bg-transparent border-2 border-white text-white px-8 py-4 rounded-lg font-semibold hover:bg-white hover:text-primary transition-colors"
              >
                View Leaderboard
              </Link>
            </div>
          </div>
        </div>
      </section>

      {/* Features Section */}
      <section className="py-16 bg-white">
        <div className="container mx-auto px-4">
          <h2 className="text-3xl font-bold text-center mb-12">Why Choose Our Quiz Platform?</h2>
          <div className="grid md:grid-cols-3 gap-8">
            <div className="text-center">
              <Brain className="w-16 h-16 mx-auto mb-4 text-primary" />
              <h3 className="text-xl font-semibold mb-3">Challenging Questions</h3>
              <p className="text-gray-600">
                Carefully curated questions covering all aspects of cooking, 
                from basic techniques to advanced culinary knowledge.
              </p>
            </div>
            <div className="text-center">
              <Clock className="w-16 h-16 mx-auto mb-4 text-primary" />
              <h3 className="text-xl font-semibold mb-3">Timed Challenges</h3>
              <p className="text-gray-600">
                Test your knowledge under pressure with our timed quiz format. 
                Perfect for quick learning sessions.
              </p>
            </div>
            <div className="text-center">
              <Trophy className="w-16 h-16 mx-auto mb-4 text-primary" />
              <h3 className="text-xl font-semibold mb-3">Earn Achievements</h3>
              <p className="text-gray-600">
                Unlock badges and achievements as you progress. 
                Compete on the global leaderboard!
              </p>
            </div>
          </div>
        </div>
      </section>

      {/* Featured Quizzes */}
      <section className="py-16 bg-gray-50">
        <div className="container mx-auto px-4">
          <h2 className="text-3xl font-bold text-center mb-12">Featured Quizzes</h2>
          {featuredQuizzes.length > 0 ? (
            <div className="grid md:grid-cols-3 gap-6">
              {featuredQuizzes.map((quiz) => (
                <QuizCard key={quiz.id} quiz={quiz} />
              ))}
            </div>
          ) : (
            <p className="text-center text-gray-600">No quizzes available yet. Check back soon!</p>
          )}
          <div className="text-center mt-8">
            <Link
              href="/quizzes"
              className="inline-block bg-primary text-white px-8 py-3 rounded-lg font-semibold hover:bg-primary-dark transition-colors"
            >
              View All Quizzes
            </Link>
          </div>
        </div>
      </section>

      {/* Categories Section */}
      <section className="py-16 bg-white">
        <div className="container mx-auto px-4">
          <h2 className="text-3xl font-bold text-center mb-12">Browse by Category</h2>
          <CategoryFilter categories={categories} selectedCategory={null} />
        </div>
      </section>
    </div>
  )
}